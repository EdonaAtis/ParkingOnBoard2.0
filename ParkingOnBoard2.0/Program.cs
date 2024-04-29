using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkingOnBoard2._0;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Welcome to Parking Management System");

        using (var context = new ParkingContext())
        {
            var street1 = new Street { Name = "28 Nentori", NumberOfSides = 2, TotalParkingSlots = 50, IsClosed = false };
            var street2 = new Street { Name = "Luan Haradinaj", NumberOfSides = 1, TotalParkingSlots = 15, IsClosed = false };
            var street3 = new Street { Name = "Sheshi", NumberOfSides = 2, TotalParkingSlots = 60, IsClosed = false };

            context.Streets.Add(street1);
            context.Streets.Add(street2);

            context.SaveChanges();


            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Manage Information on Streets");
                Console.WriteLine("2. Manage Parking Slots");
                Console.WriteLine("3. Parking");
                Console.WriteLine("4. Statistics");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");
                int option;
                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        ManageStreets(context);
                        break;
                    case 2:
                        ManageParkingSlots(context);
                        break;
                    case 3:
                        Park(context);
                        break;
                    case 4:
                        DisplayStatistics(context);
                        break;
                    case 5:
                        Console.WriteLine("Exiting program.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }

    private static void ManageStreets(ParkingContext context)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Manage Information on Streets:");
            Console.WriteLine("1. Add a street");
            Console.WriteLine("2. Close a street");
            Console.WriteLine("3. Validate a street");
            Console.WriteLine("4. Back to main menu");
            Console.Write("Select an option: ");
            int option;
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }

            switch (option)
            {
                case 1:
                    AddStreet(context);
                    break;
                case 2:
                    CloseStreet(context);
                    break;
                case 3:
                    ValidateStreet(context);
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddStreet(ParkingContext context)
    {
        Console.Write("Enter street name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter number of sides available for parking (1 or 2): ");
        int numberOfSides;
        if (!int.TryParse(Console.ReadLine(), out numberOfSides) || (numberOfSides != 1 && numberOfSides != 2))
        {
            Console.WriteLine("Invalid input. Number of sides must be 1 or 2.");
            return;
        }

        Console.Write("Enter total valid car parking slots: ");
        int totalParkingSlots;
        if (!int.TryParse(Console.ReadLine(), out totalParkingSlots) || totalParkingSlots <= 0)
        {
            Console.WriteLine("Invalid input. Total parking slots must be a positive integer.");
            return;
        }

        var street = new Street
        {
            Name = name,
            NumberOfSides = numberOfSides,
            TotalParkingSlots = totalParkingSlots
        };

        context.Streets.Add(street);
        context.SaveChanges();

        Console.WriteLine("Street added successfully.");
    }

    private static void CloseStreet(ParkingContext context)
    {
        Console.WriteLine("Available streets:");
        foreach (var s in context.Streets)
        {
            Console.WriteLine(s.Name);
        }
        Console.Write("Enter street name to close: ");
        string name = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == name);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        street.IsClosed = true;
        context.SaveChanges();

        Console.WriteLine("Street closed successfully.");
    }

    private static void ValidateStreet(ParkingContext context)
    {
        Console.Write("Enter street name to validate: ");
        string name = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == name);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        street.IsClosed = false;
        context.SaveChanges();

        Console.WriteLine("Street validated successfully.");
    }

    private static void ManageParkingSlots(ParkingContext context)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Manage Parking Slots:");
            Console.WriteLine("1. Add a parking slot to a street");
            Console.WriteLine("2. Remove a parking slot from a street");
            Console.WriteLine("3. Close a parking slot");
            Console.WriteLine("4. Validate a parking slot");
            Console.WriteLine("5. Back to main menu");
            Console.Write("Select an option: ");
            int option;
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }

            switch (option)
            {
                case 1:
                    AddParkingSlot(context);
                    break;
                case 2:
                    RemoveParkingSlot(context);
                    break;
                case 3:
                    CloseParkingSlot(context);
                    break;
                case 4:
                    ValidateParkingSlot(context);
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddParkingSlot(ParkingContext context)
    {
        Console.WriteLine("Available streets:");
        foreach (var s in context.Streets)
        {
            Console.WriteLine(s.Name);
        }

        Console.Write("Enter street name to add parking slot: ");
        string streetName = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        Console.Write("Enter parking slot number: ");
        int slotNumber;
        if (!int.TryParse(Console.ReadLine(), out slotNumber) || slotNumber <= 0)
        {
            Console.WriteLine("Invalid input. Parking slot number must be a positive integer.");
            return;
        }

        var existingSlot = context.ParkingSlots.FirstOrDefault(p => p.StreetId == street.Id && p.Number == slotNumber);
        if (existingSlot != null)
        {
            Console.WriteLine("Parking slot already exists.");
            return;
        }

        var parkingSlot = new ParkingSlot
        {
            Number = slotNumber,
            StreetId = street.Id
        };

        context.ParkingSlots.Add(parkingSlot);
        context.SaveChanges();

        Console.WriteLine("Parking slot added successfully.");
    }



    private static void RemoveParkingSlot(ParkingContext context)
    {
        Console.WriteLine("Available streets:");
        foreach (var s in context.Streets)
        {
            Console.WriteLine(s.Name);
        }

        Console.Write("Enter street name to remove parking slot from: ");
        string streetName = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        Console.Write("Enter parking slot number to remove: ");
        int slotNumber;
        if (!int.TryParse(Console.ReadLine(), out slotNumber) || slotNumber <= 0)
        {
            Console.WriteLine("Invalid input. Parking slot number must be a positive integer.");
            return;
        }

        var parkingSlot = context.ParkingSlots.FirstOrDefault(p => p.StreetId == street.Id && p.Number == slotNumber);
        if (parkingSlot == null)
        {
            Console.WriteLine("Parking slot not found.");
            return;
        }

        context.ParkingSlots.Remove(parkingSlot);
        context.SaveChanges();

        Console.WriteLine("Parking slot removed successfully.");
    }

    private static void CloseParkingSlot(ParkingContext context)
    {
        Console.WriteLine("Available streets:");
        foreach (var s in context.Streets)
        {
            Console.WriteLine(s.Name);
        }

        Console.Write("Enter street name: ");
        string streetName = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        Console.Write("Enter parking slot number to close: ");
        int slotNumber;
        if (!int.TryParse(Console.ReadLine(), out slotNumber) || slotNumber <= 0)
        {
            Console.WriteLine("Invalid input. Parking slot number must be a positive integer.");
            return;
        }

        var parkingSlot = context.ParkingSlots.FirstOrDefault(p => p.StreetId == street.Id && p.Number == slotNumber);
        if (parkingSlot == null)
        {
            Console.WriteLine("Parking slot not found.");
            return;
        }

        parkingSlot.IsValid = false;
        context.SaveChanges();

        Console.WriteLine("Parking slot closed successfully.");
    }

    private static void ValidateParkingSlot(ParkingContext context)
    {
        Console.Write("Enter street name: ");
        string streetName = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        Console.Write("Enter parking slot number to validate: ");
        int slotNumber;
        if (!int.TryParse(Console.ReadLine(), out slotNumber) || slotNumber <= 0)
        {
            Console.WriteLine("Invalid input. Parking slot number must be a positive integer.");
            return;
        }

        var parkingSlot = context.ParkingSlots.FirstOrDefault(p => p.StreetId == street.Id && p.Number == slotNumber);
        if (parkingSlot == null)
        {
            Console.WriteLine("Parking slot not found.");
            return;
        }

        parkingSlot.IsValid = true;
        context.SaveChanges();

        Console.WriteLine("Parking slot validated successfully.");
    }

    private static void Park(ParkingContext context)
    {
        Console.Write("At what street do you wish to park (enter street name or * for any street): ");
        string streetName = Console.ReadLine()!;

        IQueryable<ParkingSlot> availableSlots;
        if (streetName == "*")
        {
            availableSlots = context.ParkingSlots.Where(p => p.IsValid);
        }
        else
        {
            var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
            if (street == null)
            {
                Console.WriteLine("Street not found.");
                return;
            }

            availableSlots = context.ParkingSlots.Where(p => p.StreetId == street.Id && p.IsValid);
        }

        if (!availableSlots.Any())
        {
            Console.WriteLine("No available parking slots.");
            return;
        }

        Console.WriteLine("Available parking slots:");
        foreach (var slot in availableSlots)
        {
            Console.WriteLine($"Slot Number: {slot.Number}");
        }

        Console.Write("Enter the slot number to park: ");
        int selectedSlotNumber;
        if (!int.TryParse(Console.ReadLine(), out selectedSlotNumber))
        {
            Console.WriteLine("Invalid input. Slot number must be a positive integer.");
            return;
        }

        var selectedSlot = availableSlots.FirstOrDefault(p => p.Number == selectedSlotNumber);
        if (selectedSlot == null)
        {
            Console.WriteLine("Selected slot not found.");
            return;
        }

        selectedSlot.IsValid = false;
        context.SaveChanges();

        Console.WriteLine("Parked successfully.");
    }

    private static void DisplayStatistics(ParkingContext context)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Statistics:");
            Console.WriteLine("1. Street statistics");
            Console.WriteLine("2. City statistics");
            Console.WriteLine("3. Back to main menu");
            Console.Write("Select an option: ");
            int option;
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }

            switch (option)
            {
                case 1:
                    DisplayStreetStatistics(context);
                    break;
                case 2:
                    DisplayCityStatistics(context);
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void DisplayStreetStatistics(ParkingContext context)
    {
        Console.WriteLine("Available streets:");
        foreach (var s in context.Streets)
        {
            Console.WriteLine(s.Name);
        }

        Console.Write("Enter street name: ");
        string streetName = Console.ReadLine()!;

        var street = context.Streets.FirstOrDefault(s => s.Name == streetName);
        if (street == null)
        {
            Console.WriteLine("Street not found.");
            return;
        }

        int totalSlots = street.TotalParkingSlots;
        int occupiedSlots = context.ParkingSlots.Count(p => p.StreetId == street.Id && !p.IsValid);
        int invalidSlots = context.ParkingSlots.Count(p => p.StreetId == street.Id && !p.IsValid);

        double percentFree = (double)(totalSlots - occupiedSlots) / totalSlots * 100;
        double percentOccupied = (double)occupiedSlots / totalSlots * 100;
        double percentInvalid = (double)invalidSlots / totalSlots * 100;

        Console.WriteLine($"Street Statistics for {streetName}:");
        Console.WriteLine($"% of Free Slots: {percentFree}%");
        Console.WriteLine($"% of Occupied Slots: {percentOccupied}%");
        Console.WriteLine($"% of Invalid Slots: {percentInvalid}%");
    }

    private static void DisplayCityStatistics(ParkingContext context)
    {
        var streets = context.Streets.ToList();
        if (!streets.Any())
        {
            Console.WriteLine("No streets found.");
            return;
        }

        foreach (var street in streets)
        {
            int totalSlots = street.TotalParkingSlots;
            int occupiedSlots = context.ParkingSlots.Count(p => p.StreetId == street.Id && !p.IsValid);

            double percentOccupied = (double)occupiedSlots / totalSlots * 100;

            Console.WriteLine($"Street: {street.Name}");
            Console.WriteLine($"% of Occupied Slots: {percentOccupied}%");
        }

        var lessOccupiedStreets = streets.Where(s => context.ParkingSlots.Count(p => p.StreetId == s.Id && !p.IsValid) <= s.TotalParkingSlots * 0.25);
        if (lessOccupiedStreets.Any())
        {
            Console.WriteLine("Less occupied streets:");
            foreach (var street in lessOccupiedStreets)
            {
                Console.WriteLine(street.Name);
            }
        }


        var heavyOccupiedStreets = streets.Where(s => context.ParkingSlots.Count(p => p.StreetId == s.Id && !p.IsValid) >= s.TotalParkingSlots * 0.75);
        if (heavyOccupiedStreets.Any())
        {
            Console.WriteLine("Heavy occupied streets:");
            foreach (var street in heavyOccupiedStreets)
            {
                Console.WriteLine(street.Name);
            }
        }
    }
}

