﻿using System;
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

            PointOfEntry.Run();
        }
    }
}
