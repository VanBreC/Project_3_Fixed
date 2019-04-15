using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Project_3_Fixed
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<Student> studentList = new List<Student>();
        public Window1()
        {
            InitializeComponent();

            studentList = readData();
            Label18.Content = studentList.Count.ToString();
        }
        

        private void ChkWorker_Checked(object sender, EventArgs e)
        {
            
            if (Label4.Visibility == Visibility.Hidden)
            {
                Label4.Visibility = Visibility.Visible;
                Label5.Visibility = Visibility.Visible;
                hours.Visibility = Visibility.Visible;
                Pay.Visibility = Visibility.Visible;
            }
        }
        private void ChkAthlete_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAtlete.IsChecked == true)
            {
                Label4.Visibility = Visibility.Hidden;
                Label5.Visibility = Visibility.Hidden;
                hours.Visibility = Visibility.Hidden;
                Pay.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (newFName.Text == "")
            {
                MessageBox.Show("Please Enter the Students First Name");
            }
            if (newLName.Text == "")
            {
                MessageBox.Show("Please Enter the Students Last Name");
            }
            if (newID.Text == "")
            {
                MessageBox.Show("Please Enter the Students ID");
            }
            try
            {

                FileStream file = new FileStream("Residence Hall.csv", FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(file);

                Random randomSelector = new Random();
                string fname = newFName.Text;
                string lname = newLName.Text;
                string ID = newID.Text;
                int rent = 0;
                int floor = 0;
                int dorm = 0;
                string stdntType = "";
                

                if (chkScholorship.IsChecked == true)
                {
                    rent = 100;
                    floor = randomSelector.Next(7, 8);
                    dorm = randomSelector.Next(700, 899);
                    stdntType = "Scholorship";
                }
                else if (chkAtlete.IsChecked == true)
                {
                    rent = 1200;
                    floor = randomSelector.Next(4, 6);
                    dorm = randomSelector.Next(400, 699);
                    stdntType = "Athlete";
                }
                else if (chkWorker.IsChecked == true)
                {
                    int monthlyHours = int.Parse(hours.Text);
                    int hourlyPay = int.Parse(Pay.Text);
                    rent = 1245-((monthlyHours * hourlyPay) / 2);
                    floor = randomSelector.Next(1, 3);
                    dorm = randomSelector.Next(100, 399);
                    stdntType = "Worker";
                }
                else
                {
                    MessageBox.Show("Please Select What Kind of Student You Are");
                }

                writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", ID, fname, lname, rent, floor, dorm, stdntType);

                writer.Close();
                file.Close();

                newFName.Clear();
                newLName.Clear();
                newID.Clear();
                

            }
            catch (Exception i)
            {
                Console.WriteLine(i.StackTrace);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int rent;
                int idNumber = Convert.ToInt32(idSearch.Text);

                var stdntFile =
                    from stdnt in studentList
                    where idNumber == stdnt.idNum
                    select stdnt;

                foreach (var stdnt in stdntFile)
                {
                    if (stdnt.idNum == idNumber)
                    {
                        rent = stdnt.rent;
                        Label13.Content = Convert.ToString(stdnt.idNum);
                        Label11.Content = stdnt.fname;
                        Label12.Content = stdnt.lname;
                        Label15.Content = Convert.ToString(stdnt.dormNum);
                        Label14.Content = Convert.ToString(stdnt.floor);
                        Label16.Content = rent.ToString("c");
                        Label17.Content = stdnt.studentGroup;
                    }
                    else if (stdnt.idNum != idNumber)
                    {

                    }

                    idSearch.Clear();

                }
            }
            catch (Exception i)
            {
                Console.WriteLine(i.StackTrace);
                MessageBox.Show("Please Enter an ID Number");
            }
        }
        public static List<Student> readData()
        {

            List<Student> students = new List<Student>();
            Scholar scholar;
            Athlete athlete;
            Worker worker;
            const string FILE = "Residence Hall.csv";
            const char DELIM = ',';
            string[] info;

            try
            {

                FileStream file = new FileStream(FILE, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(file);
                string headerLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {

                    info = reader.ReadLine().Split(DELIM);

                    switch (info[6])
                    {
                        case "Scholar":
                            scholar = new Scholar(Convert.ToInt32(info[0]), info[1], info[2], Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), Convert.ToInt32(info[5]), info[6]);
                            Console.WriteLine(scholar);
                            students.Add(scholar);
                            break;
                        case "Athlete":
                            athlete = new Athlete(Convert.ToInt32(info[0]), info[1], info[2], Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), Convert.ToInt32(info[5]), info[6]);
                            Console.WriteLine(athlete);
                            students.Add(athlete);
                            break;
                        case "Worker":
                            worker = new Worker(Convert.ToInt32(info[0]), info[1], info[2], Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), Convert.ToInt32(info[5]), info[6]);
                            Console.WriteLine(worker);
                            students.Add(worker);
                            break;
                        default:
                            break;
                    }
                }

                reader.Close();
                file.Close();

            }
            catch (Exception i)
            {
                Console.WriteLine(i.StackTrace);
            }

            return students;

        }
        

        
    }
    interface IRDFunctions
    {
        int Balance();
    }
    public abstract class Student : IRDFunctions
    {
        public int idNum { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public int dormNum { get; set; }
        public int floor { get; set; }
        public int rent { get; set; }
        public string studentGroup { get; set; }

        public Student()
        {

        }

        public Student(int idNum, string fname, string lname, int dormNum, int floor, int rent, string stdntGroup)
        {
            this.idNum = idNum;
            this.fname = fname;
            this.lname = lname;
            this.dormNum = dormNum;
            this.floor = floor;
            this.rent = rent;
            studentGroup = stdntGroup;
        }

        public abstract int Balance();
    }
    class Scholar : Student
    {
        public Scholar(int idNum, string fname, string lname, int dormNum, int floor, int rent, string stdntGroup) : base(idNum, fname, lname, dormNum, floor, rent, stdntGroup)
        {
            this.idNum = idNum;
            this.fname = fname;
            this.lname = lname;
            this.dormNum = dormNum;
            this.floor = floor;
            this.rent = rent;
            studentGroup = stdntGroup;
        }

        public override int Balance()
        {
            return rent;
        }
    }

    class Athlete : Student
    {
        public Athlete(int idNum, string fname, string lname, int dormNum, int floor, int rent, string stdntGroup) : base(idNum, fname, lname, dormNum, floor, rent, stdntGroup)
        {
            this.idNum = idNum;
            this.fname = fname;
            this.lname = lname;
            this.dormNum = dormNum;
            this.floor = floor;
            this.rent = rent;
            studentGroup = stdntGroup;
        }

        public override int Balance()
        {
            return rent;
        }
    }

    class Worker : Student
    {
        public Worker(int idNum, string fname, string lname, int dormNum, int floor, int rent, string stdntGroup) : base(idNum, fname, lname, dormNum, floor, rent, stdntGroup)
        {
            this.idNum = idNum;
            this.fname = fname;
            this.lname = lname;
            this.dormNum = dormNum;
            this.floor = floor;
            this.rent = rent;
            studentGroup = stdntGroup;
        }

        public override int Balance()
        {
            return rent;
        }
    }

}
