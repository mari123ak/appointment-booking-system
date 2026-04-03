using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text.Json;

//update options for json file e.g. delete

List<Appointment> appointments = new List<Appointment>();


void AddApp()
{
    Console.WriteLine("Enter name: ");
    string client = Console.ReadLine().ToLower();
    Console.WriteLine("Enter description: ");
    string clientDescrip = Console.ReadLine();


    try
    {
        Console.WriteLine("Enter date and time: ");
        DateTime clientDateTime = Convert.ToDateTime(Console.ReadLine());

        while (appointments.Any(a => a.dateAndTime == clientDateTime))
        {
            Console.WriteLine("Error, this date and time already exists. Please enter a new date and time: ");
            clientDateTime = Convert.ToDateTime(Console.ReadLine());
        }

        //stores the inputs in a list (using the class constructor).
        appointments.Add(new Appointment(client, clientDescrip, clientDateTime));
        SaveAppointments();

    }
    catch
    {
        Console.WriteLine("Please enter the correct format of DD/MM/YYYY HH:MM ");
    }
}
void ViewApp()
{
    appointments.Sort((a, b) => a.dateAndTime.CompareTo(b.dateAndTime));

    if (appointments.Count != 0)
    {

        foreach (Appointment i in appointments)
        {


            Console.WriteLine($" Client name: {i.name}\n Description: {i.description}\n Date and Time: {i.dateAndTime} \n ");

        }
    }
    else if (appointments.Count == 0)
    {

        Console.WriteLine("There are no appointments to view\n");
    }
}

void DeleteApp()
{
    Console.WriteLine("Enter client name: ");
    string deleteName = Console.ReadLine().ToLower();
    Console.WriteLine("Enter the date and time: ");
    DateTime deleteDT = Convert.ToDateTime(Console.ReadLine());

    if (appointments.Any(b => b.name == (deleteName) && (b.dateAndTime == deleteDT)))
    {
        appointments.RemoveAll((b => b.name == deleteName && b.dateAndTime == deleteDT));

        Console.WriteLine("Appointment has been deleted\n");

    }
    else
    {
        Console.WriteLine("Appointment not found\n");
    }
}

void SearchApp()
{
    Console.WriteLine("enter clients name: ");
    string nameCheck = Console.ReadLine().ToLower();

    //.Any() LINQ method checks if element matches condition.
    //Does name field contained in the list appointments contain the input from user?

    if (appointments.Any(a => a.name == nameCheck))
    { //Add all the matching names to the 'matches' list
        List<Appointment> matches = appointments.FindAll(p => p.name == nameCheck);
        foreach (Appointment i in matches)
        {

            Console.WriteLine("client: {0}, description: {1}, Date and time: {2}\n", i.name, i.description, i.dateAndTime);
        }

    }
    else
    {

        Console.WriteLine("Client does not exist\n");
    }
}



LoadAppointments();
Console.WriteLine("Appointment Booking System");
string option = "X";

//string allows user error input
while (option != "E")
{
    Console.WriteLine("Menu Options: \n (A) Add Appointment \n (B) View Appointments \n (C) Delete Appointments \n (D) Search Appointments \n (E) Exit");
    option = Console.ReadLine().ToUpper();

    Appointment appointment;


    switch (option)
    {
        case "A":
            AddApp(); break;
        case "B":
            ViewApp(); break;
        case "C":
            DeleteApp(); break;
        case "D":
            SearchApp(); break;
        case "E":
            Console.WriteLine("Exited program"); break;
        default:
            Console.WriteLine("please pick a valid option\n"); break;

    }

}

void SaveAppointments()
{
    string jsonIn = JsonSerializer.Serialize(appointments, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("appointments.json", jsonIn);
}

void LoadAppointments()
{

    if (File.Exists("appointments.json"))
    {
        string jsonOut = File.ReadAllText("appointments.json");
        appointments = JsonSerializer.Deserialize<List<Appointment>>(jsonOut);
    }
}

public class Appointment
{
    public string name { get; set; }
    public string description { get; set; }
    public DateTime dateAndTime { get; set; }

    public Appointment(string Name, string Description, DateTime DateAndTime)
    {
        name = Name;
        description = Description;
        dateAndTime = DateAndTime;

    }
}