using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test Adopt()
            //Animal animal = new Animal();
            //Client client = new Client();
            //client.ClientId = 1015;
            //animal.AnimalId = 1019;

            //Query.Adopt(animal, client);
            //Console.ReadLine();

            //Test GetPendingAdoptions()
            //IQueryable<Adoption> adoptions = Query.GetPendingAdoptions();
            //Console.ReadLine();

            //Test UpdateAdoption()
            //Adoption adoption = new Adoption();
            //adoption.AnimalId = 1019;
            //Query.UpdateAdoption(true, adoption);

            //Test RemoveAdoption
            //Query.RemoveAdoption(1017, 1014);

<<<<<<< HEAD
            PointOfEntry.Run();
=======
            //Test GetShots
            Animal animal = new Animal();
            animal.AnimalId = 1014;
            Query.GetShots(animal);
            //PointOfEntry.Run();
>>>>>>> 1557e124ca605942d7b5beb337e36655f94a355f
        }
    }
}
