using Sodao.Juketool.Share.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.Newtonsoft
{
    [Main]
    public class NewtonsoftCase : ICase
    {
        public void Init()
        {

        }

        public void Run()
        {
            AveragePriceCondition averagePrice = new AveragePriceCondition { Start = 10, End = 20 };
            string json = averagePrice.Serialize();
            AveragePriceCondition newAveragePrice = new AveragePriceCondition();
            newAveragePrice.Deserialize(json);

            Console.WriteLine(json);
            Console.WriteLine(newAveragePrice.Start + " " + newAveragePrice.End);

            GenderCondition gender = new GenderCondition();
            gender.Add(CustomerGender.Female.ToString());
            gender.Add(CustomerGender.Male.ToString());
            json = gender.Serialize();
            Console.WriteLine(json);
            GenderCondition newGender = new GenderCondition();
            newGender.Deserialize(json);
            Console.WriteLine(string.Join(",",newGender));

            ProductsCondition products = new ProductsCondition();
            products.Add(new ProductSelect { Id = 1000, Title = "商品A" });
            products.Add(new ProductSelect { Id = 1001, Title = "商品B" });
            json = products.Serialize();
            ProductsCondition newProducts = new ProductsCondition();
            newProducts.Deserialize(json);

            ConditionCollection conditionCollection = new ConditionCollection();
            conditionCollection.Add(averagePrice);
            conditionCollection.Add(gender);
            conditionCollection.Add(products);

            json = conditionCollection.Serialize();

            Console.WriteLine(json);
            ConditionCollection newConditionCollection = new ConditionCollection();
            newConditionCollection.Deserialize(json);
            
        }
    }
}