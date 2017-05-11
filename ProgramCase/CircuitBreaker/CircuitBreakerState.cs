using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgramCase.CircuitBreaker
{
    public class CircuitBreakerContext
    {
        private CircuitBreakerState _openState;
        private CircuitBreakerState _closeState;
        private CircuitBreakerState _halfOpenState;
        private ReaderWriterLockSlim _lockSlim;
        private Action _protectAction;
        private CircuitBreakerState _currentState;

        public CircuitBreakerContext(Action protectAction)
        {
            _protectAction = protectAction ?? throw new ArgumentNullException(nameof(protectAction));
            _closeState = new CloseState(this, new Tuple<TimeSpan, int>(TimeSpan.FromMinutes(1), 3));
            _openState = new OpenState(this, TimeSpan.FromMinutes(2));
            _halfOpenState = new HalfOpenState(this,10);
            _lockSlim = new ReaderWriterLockSlim();
        }

        public void Execute()
        {
            _lockSlim.UseReaderLock(() =>
            {
                try
                {
                    _currentState.ProcessBefore();
                    _protectAction();
                    _currentState.ProcessSuccess();
                }
                catch (RemoteResourceException)
                {
                    _currentState.ProcessFail();
                    throw;
                }
            });
        }

        public void MoveToCloseState()
        {
            _lockSlim.UseWriterLock(() =>
            {
                _currentState.Clear();
                _currentState = _closeState;
                _currentState.Initialize();
            });
        }

        public void MoveToOpenState()
        {
            _lockSlim.UseWriterLock(() =>
            {
                _currentState.Clear();
                _currentState = _openState;
                _currentState.Initialize();
            });
        }

        public void MoveToHalfOpenState()
        {
            _lockSlim.UseWriterLock(() =>
            {
                _currentState.Clear();
                _currentState = _halfOpenState;
                _currentState.Initialize();
            });
        }
    }

    public abstract class CircuitBreakerState
    {
        protected CircuitBreakerContext Context { get; private set; }

        protected CircuitBreakerState(CircuitBreakerContext context)
        {
            Context = context;
        }

        public abstract void Initialize();

        public abstract void Clear();

        public abstract void ProcessBefore();

        public abstract void ProcessSuccess();

        public abstract void ProcessFail();
    }

    public class CloseState : CircuitBreakerState
    {
        private readonly Tuple<TimeSpan, int> _allowFailSetting;
        private Timer _timer;
        private int _currrentFailTimes;

        public CloseState(CircuitBreakerContext context, Tuple<TimeSpan, int> allowFailSetting) : base(context)
        {
            if (_allowFailSetting == null) throw new ArgumentNullException(nameof(allowFailSetting));
            if (_allowFailSetting.Item1 <= TimeSpan.Zero || _allowFailSetting.Item2 <= 0)
            {
                throw new ArgumentOutOfRangeException("时间间隔不能小于0，允许失败的次数不能小于等于0次！");
            }

            _allowFailSetting = allowFailSetting;
            _timer = new Timer(CheckAllowTimes, null, -1, -1);
        }

        private void CheckAllowTimes(object state)
        {
            ResetFailTimes();
        }

        private void ResetFailTimes()
        {
            Interlocked.Exchange(ref _currrentFailTimes, 0);
        }

        public override void Initialize()
        {
            _timer.Restart(_allowFailSetting.Item1, _allowFailSetting.Item1);
        }

        public override void ProcessFail()
        {
            int failTimes = Interlocked.Increment(ref _currrentFailTimes);
            if (failTimes > _allowFailSetting.Item2)
            {
                Context.MoveToOpenState();
            }
        }

        public override void ProcessSuccess()
        {
        }

        public override void ProcessBefore()
        {
        }

        public override void Clear()
        {
            _timer.Stop();
            ResetFailTimes();
        }
    }

    public class OpenState : CircuitBreakerState
    {
        private Timer _switchHalfOpenTimer;
        private TimeSpan _switchHalfOpenTimeSpan;

        public OpenState(CircuitBreakerContext context, TimeSpan switchHalfOpenTimeSpan) : base(context)
        {
            if (switchHalfOpenTimeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("时间间隔不能小于0");
            }
            _switchHalfOpenTimeSpan = switchHalfOpenTimeSpan;
           _switchHalfOpenTimer = new Timer(SwitchHalfOpenState,null,-1,-1);
        }

        private void SwitchHalfOpenState(object state)
        {
            Context.MoveToHalfOpenState();
        }

        public override void Initialize()
        {
            _switchHalfOpenTimer.Restart(_switchHalfOpenTimeSpan, _switchHalfOpenTimeSpan);
        }

        public override void ProcessFail()
        {
            throw new RemoteResourceException("熔断器开启状态下无法执行方法");
        }

        public override void ProcessSuccess()
        {
            throw new RemoteResourceException("熔断器开启状态下无法执行方法");
        }

        public override void ProcessBefore()
        {
            throw new RemoteResourceException("熔断器开启状态下无法执行方法");
        }

        public override void Clear()
        {
            _switchHalfOpenTimer.Stop();
        }
    }

    public class HalfOpenState : CircuitBreakerState
    {
        private readonly int _allowExecuteNums;
        private int _currentExecuteNums;
        private int _currentExecuteSuccessTimes;

        public HalfOpenState(CircuitBreakerContext context,int allowExecuteNums) : base(context)
        {
            if (allowExecuteNums <= 0) throw new ArgumentOutOfRangeException("允许请求的次数不能少于0次");

            _allowExecuteNums = allowExecuteNums;
        }

        public override void Initialize()
        {
        }

        public override void ProcessFail()
        {
            Context.MoveToOpenState();
        }

        public override void ProcessSuccess()
        {
            if (Interlocked.Increment(ref _currentExecuteSuccessTimes) >= _allowExecuteNums)
            {
                Context.MoveToCloseState();
            }
        }

        public override void ProcessBefore()
        {
            int executeNums = Interlocked.Increment(ref _currentExecuteNums);
            if(executeNums > _allowExecuteNums)
            {
                throw new RemoteResourceException("熔断器半开启状态下无法执行方法");
            }
        }

        private void ResetAllowExecuteNums()
        {
            Interlocked.Exchange(ref _currentExecuteNums, 0);
            Interlocked.Exchange(ref _currentExecuteSuccessTimes, 0);
        }

        public override void Clear()
        {
            ResetAllowExecuteNums();
        }
    }
}