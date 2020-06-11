using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }

        internal static void UpdateEmployee(Employee employeeWithUpdates)
        {
            // find corresponding Employee from Db
            Employee employeeFromDb = null;

            try
            {
                employeeFromDb = db.Employees.Where(c => c.EmployeeNumber == employeeWithUpdates.EmployeeNumber).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No employees have that employee number.");
                Console.WriteLine("No update have been made.");
                return;
            }

            // update employeeFromDb information with the values on employeeWithUpdates 
            employeeFromDb.FirstName = employeeWithUpdates.FirstName;
            employeeFromDb.LastName = employeeWithUpdates.LastName;
            employeeFromDb.Email = employeeWithUpdates.Email;

            // submit changes
            db.SubmitChanges();
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName != null;
        }

        internal static bool CheckEmployeeNumberExist(Employee employee)
        {
            Employee employeeWithNumber = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();

            return employeeWithNumber != null;
        }


        //// TODO Items: ////

        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch (crudOperation)
            {
                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "read":
                    try
                    {
                        Employee readEmployeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
                        UserInterface.DisplayEmployeeInfo(readEmployeeFromDb);
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case "update":
                    UpdateEmployee(employee);
                    break;
                case "delete":
                    Employee employeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber && e.LastName == employee.LastName).FirstOrDefault();
                    db.Employees.DeleteOnSubmit(employeeFromDb);
                    db.SubmitChanges();
                    break;
                default:
                    break;
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
           return db.Animals.Where(a => a.AnimalId == id).Single();
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            // find corresponding Animal from Db
            Animal animalFromDb = null;

            try
            {
                animalFromDb = db.Animals.Where(c => c.AnimalId == animalId).Single();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Animal not found. \nNo updates have been made.");
                return;
            }

            //For each corresponding trait, ex(1, cat), update animalFromDb information with the values
            foreach (KeyValuePair<int, string> trait in updates)
            {
                switch (trait.Key)
                {
                    case 1:
                        //animalFromDb.Category = GetCategoryId(trait.Value);
                        break;
                    case 2:
                        animalFromDb.Name = trait.Value;
                        break;
                    case 3:
                        animalFromDb.Age = int.Parse(trait.Value);
                        break;
                    case 4:
                        animalFromDb.Demeanor = trait.Value;
                        break;
                    case 5:
                        animalFromDb.KidFriendly = bool.Parse(trait.Value);
                        break;
                    case 6:
                        animalFromDb.PetFriendly = bool.Parse(trait.Value);
                        break;
                    case 7:
                        animalFromDb.Weight = int.Parse(trait.Value);
                        break;
                    default:
                        break;
                }
            }
            // submit changes
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            var animals = db.Animals.ToList();
            

            foreach (KeyValuePair<int, string> animalUpdate in updates)
            {
                switch (animalUpdate.Key)
                {
                    case 1:
                        animals = animals.Where(s => s.CategoryId == GetCategoryId(animalUpdate.Value)).ToList();
                        break;
                    case 2:
                        animals = animals.Where(s => s.Name == animalUpdate.Value).ToList();
                        break;
                    case 3:
                        animals = animals.Where(s => s.Age.ToString() == animalUpdate.Value).ToList();
                        break;
                    case 4:
                        animals = animals.Where(s => s.Demeanor == animalUpdate.Value).ToList();
                        break;
                    case 5:
                        animals = animals.Where(s => s.KidFriendly.ToString() == animalUpdate.Value).ToList();
                        break;
                    case 6:
                        animals = animals.Where(s => s.PetFriendly.ToString() == animalUpdate.Value).ToList();
                        break;
                    case 7:
                        animals = animals.Where(s => s.Weight.ToString() == animalUpdate.Value).ToList();
                        break;
                    case 8:
                        animals = animals.Where(s => s.AnimalId.ToString() == animalUpdate.Value).ToList();
                        break;
                }
                

            }
            var queryable = animals.AsQueryable();

            return queryable;

        }
         
        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryNam)
        {
            Category category = db.Categories.Where(c => c.Name == categoryNam).FirstOrDefault();

            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)
        {
            Room room = db.Rooms.Where(a => a.AnimalId == animalId).FirstOrDefault();

            return room;

        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            DietPlan dietplan = db.DietPlans.Where(d => d.Name == dietPlanName).FirstOrDefault();

            return dietplan.DietPlanId;
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            Adoption adoption = new Adoption();
            adoption.ClientId = client.ClientId;
            adoption.AnimalId = animal.AnimalId;
            adoption.ApprovalStatus = "Pending";
            adoption.AdoptionFee = 75;
            adoption.PaymentCollected = false;

            try
            {
                db.Adoptions.InsertOnSubmit(adoption);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
              
            }

        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            try
            {
                IQueryable<Adoption> adoption = db.Adoptions.Where(a => a.ApprovalStatus == "Pending");
                return adoption;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            try
            {
                Adoption adoption1 = db.Adoptions.Where(x => x.AnimalId == adoption.AnimalId).FirstOrDefault();

                if (isAdopted)
                {
                    adoption1.ApprovalStatus = "Approved";
                    adoption1.PaymentCollected = true;
                }
                else
                {
                    adoption1.ApprovalStatus = "Denied";
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
            }

           

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
          
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            try
            {
                Adoption adoption = db.Adoptions.Single(e => e.AnimalId == animalId && e.ClientId == clientId && e.ApprovalStatus != "Approved");
                db.Adoptions.DeleteOnSubmit(adoption);
                db.SubmitChanges();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);

            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            try
            {
                IQueryable<AnimalShot> shots = db.AnimalShots.Where(a => a.AnimalId == animal.AnimalId);
                return shots;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            try
            {
                //Check if shot exists in DB
                Shot shots  = db.Shots.Where(x => x.Name == shotName).FirstOrDefault();

                //If shot does not exist, add to DB
                if(shots == null)
                {
                    Shot newShots = new Shot();
                    newShots.Name = shotName;

                    db.Shots.InsertOnSubmit(newShots);
                    db.SubmitChanges();
                    shots = newShots;
                }
                var shotId = shots.ShotId;
                

                //Add new AnimalShot to DB, getting shotId from Shot Table and AnimalId from Animal Table
               // AnimalShot animalShot = new AnimalShot();
                //animalShot.ShotId = shots.ShotId;
               // animalShot.AnimalId = animal.AnimalId;
               // animalShot.DateReceived = DateTime.Today;

                //db.AnimalShots.InsertOnSubmit(animalShot);

                AnimalShot animalShot = db.AnimalShots.Where(s => s.AnimalId == animal.AnimalId && shotId == s.ShotId).FirstOrDefault();
             
                if (animalShot != null)
                {
                    animalShot.DateReceived = DateTime.Now;
                    db.SubmitChanges();
                }
                else
                {
                    AnimalShot shot = new AnimalShot();
                    shot.AnimalId = animal.AnimalId;
                    shot.ShotId = shotId;
                    shot.DateReceived = DateTime.Now;
                    db.AnimalShots.InsertOnSubmit(shot);
                    db.SubmitChanges();
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
            }
            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}