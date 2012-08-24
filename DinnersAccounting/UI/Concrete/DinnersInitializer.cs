using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DA.Dinners.Domain;
using DA.Dinners.Domain.Concrete;
using DA.Dinners.Model;

//using DA.Dinners.Domain.Concrete;

namespace UI.Concrete
{

    public class DinnersInitializer : DropCreateDatabaseIfModelChanges<DADinnersDomainContext>
    {
        protected override void Seed(DADinnersDomainContext context)
        {
            AddConstraints(context);

            InitializingByTestValues(context);
        }

        private void InitializingByTestValues(DADinnersDomainContext context)
        {
            PersonRepository prep = new PersonRepository();

            Person p1 = new Person { DomainName = "UNIVERSE\\ikogan" };
            Person p4 = new Person { DomainName = "UNIVERSE\\bdudnik" };

            Role role = new Role { RoleName = "Admin" };

            p4.Roles.Add(role);

            prep.InsertOrUpdate(p1);
            prep.InsertOrUpdate(p4);
            prep.Save();

            ContinuousProposition cp1 = new ContinuousProposition { StartDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday), EndDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7) };
            cp1.Init();

            Product pd1 = new Product { Title = "Салат по - домашнему", Summary = "(помидоры, огурцы, масло растительное) 0,150", Price = 5.9M };
            Product pd2 = new Product { Title = "Борщ зелёный ", Summary = "0,300", Price = 8.44M };
            Product pd3 = new Product { Title = "Свекольник ", Summary = "0,300", Price = 9.09M };
            Product pd4 = new Product { Title = "Медальоны из телятины по - итальянски  ", Summary = "(на крутонах) 0,100", Price = 13.91M };

            Product k1 = new Product { Title = "Бизнес – комплекс № 2", Summary = "Капуста с помидорами 0,050 Суп \"Харчо\" 0,300 Сосиски отварные 0,100 Каша гречневая 0,200", Price = 21, isComplex = true };
            Product k2 = new Product { Title = "Комплекс – профессионал № 1", Summary = "Салат \"Дамский каприз\" (ветчина, капуста, помидоры, огурцы, майонез) 0,100 Суп \"Харчо\" 0,300 Эскалоп с помидорами 0,100 Картофель тушёный 0,200", Price = 25, isComplex = true };
            Product k3 = new Product { Title = "Комплекс – профессионал № 2", Summary = "Капуста с помидорами 0,050 Суп \"Харчо\" 0,300 Сосиски отварные 0,100 Каша гречневая 0,200", Price = 16, isComplex = true };

            cp1.Products.Add(pd1);
            cp1.Products.Add(pd2);

            foreach (var DayProposition in cp1.DayPropositions)
            {
                DayProposition.Products.Add(pd1.Clone());
                DayProposition.Products.Add(pd4.Clone());
                DayProposition.Products.Add(k1.Clone());
                DayProposition.Products.Add(k2.Clone());
                DayProposition.Products.Add(k3.Clone());
            }

            ContinuousProposition cp2 = new ContinuousProposition { StartDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(8), EndDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(15) };
            cp2.Init();
            cp2.Products.Add(pd4.Clone());
            cp2.Products.Add(pd3.Clone());
            foreach (var DayProposition in cp2.DayPropositions)
            {
                DayProposition.Products.Add(pd2.Clone());
                DayProposition.Products.Add(pd4.Clone());
                DayProposition.Products.Add(k1.Clone());
                DayProposition.Products.Add(k2.Clone());
                DayProposition.Products.Add(k3.Clone());
            }

            ContinuousProposition cp3 = new ContinuousProposition { StartDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(16), EndDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(23) };
            cp3.Init();
            cp3.Products.Add(pd1.Clone());
            cp3.Products.Add(pd4.Clone());

            foreach (var DayProposition in cp3.DayPropositions)
            {
                DayProposition.Products.Add(pd3.Clone());
                DayProposition.Products.Add(pd4.Clone());
                DayProposition.Products.Add(k1.Clone());
                DayProposition.Products.Add(k2.Clone());
                DayProposition.Products.Add(k3.Clone());
            }

            PropositionRepository repo = new PropositionRepository();
            repo.InsertOrUpdate(cp1);
            repo.InsertOrUpdate(cp2);
            repo.InsertOrUpdate(cp3);
            repo.Save();

            Person p = context.People.SingleOrDefault(pr => pr.DomainName == "UNIVERSE\\bdudnik");

            Order order1 = new Order { Date = DateTime.Now.AddDays(1) };
            order1.Statuses.Add(new OrderStatus { Date = order1.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order1.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            Order order2 = new Order { Date = DateTime.Now.AddDays(3) };
            order2.Statuses.Add(new OrderStatus { Date = order2.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order2.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            Order order3 = new Order { Date = DateTime.Now.AddDays(4) };
            order3.Statuses.Add(new OrderStatus { Date = order3.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order3.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            Order order4 = new Order { Date = DateTime.Now.AddDays(1) };
            order4.Statuses.Add(new OrderStatus { Date = order4.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order4.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            Order order5 = new Order { Date = DateTime.Now.AddDays(3) };
            order5.Statuses.Add(new OrderStatus { Date = order5.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order5.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            Order order6 = new Order { Date = DateTime.Now.AddDays(4) };
            order6.Statuses.Add(new OrderStatus { Date = order6.Date, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });
            order6.OrderDetail.Add(new OrderDetail { Product = context.Products.First(), Quantity = 1 });

            context.Orders.Add(order1);
            context.Orders.Add(order2);
            context.Orders.Add(order3);
            context.Orders.Add(order4);
            context.Orders.Add(order5);
            context.Orders.Add(order6);

            p.Orders.Add(order1);
            p.Orders.Add(order2);
            p.Orders.Add(order3);

            Person p2 = context.People.SingleOrDefault(pr => pr.DomainName == "UNIVERSE\\ikogan");

            p2.Orders.Add(order4);
            p2.Orders.Add(order5);
            p2.Orders.Add(order6);

            context.Entry(context.People.Find(p.Id)).State = System.Data.EntityState.Modified;
            context.Entry(context.People.Find(p2.Id)).State = System.Data.EntityState.Modified;
            context.SaveChanges();

            //            prep.InsertOrUpdate(p);
            //            prep.InsertOrUpdate(p2);
            //            prep.Save();

            //ContinuousPropositionRepository cpr = new ContinuousPropositionRepository();
            //cpr.Delete(1);
            //cpr.Save();
            //ProductRepository prod_rep = new ProductRepository();
            //Product pd1_ = prod_rep.Find(pd1.Id);
            //Product updated_Product = new Product { Id = pd1_.Id, isComplex = pd1_.isComplex, Price = pd1_.Price, Summary = pd1_.Summary, Title = pd1_.Title };

            //prod_rep.InsertOrUpdate(prod_rep.Find(pd1.Id));
            //prod_rep.Save();

            //prod_rep.InsertOrUpdate(updated_Product);
            //prod_rep.Save();
            //ContinuousProposition cp_test = (ContinuousProposition)new PropositionRepository().AllIncluding(prop => prop.Products).ToList().Find(pp => pp.Id == cp1.Id);//; Find(cp1.Id);
            //ContinuousProposition cp_test = context.ContinuousPropositions.Include(p => p.Products).ToList().Find(pp => pp.Id == cp1.Id);
            //int i = cp_test.DayPropositions.ToList().Count;
        }

        private void AddConstraints(DADinnersDomainContext context)
        {
            //Additional constraints:
            //context.Database.ExecuteSqlCommand("ALTER TABLE People ADD CONSTRAINT PersonAccount UNIQUE(PersonAccount_Id)");
        }
    }
}

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = dt.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
            diff += 7;
        }

        return dt.AddDays(-1 * diff).Date;
    }
}
