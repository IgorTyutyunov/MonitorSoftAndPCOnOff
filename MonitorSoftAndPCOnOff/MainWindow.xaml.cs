using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Threading;
using System.Data.SQLite;

namespace MonitorSoftAndPCOnOff
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String dbFileName="testDB";
        private SQLiteConnection m_dbConn = new SQLiteConnection();
        private SQLiteCommand m_sqlCmd = new SQLiteCommand();
        private int id_working=0;

     


            private void timer_Tick(object sender, EventArgs e)
        {
            addOff();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(dbFileName))
            {
                
            }
            

        }

        private void buttonAddOn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void windows_Closed(object sender, EventArgs e)
        {
            addOff();
            m_dbConn.Close();
        }
        

        private void windows_Loaded(object sender, RoutedEventArgs e)
        {


            if (System.IO.File.Exists(dbFileName))
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                addOn();

                //  установка таймера
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();

            }
            else {
                CreateDatabase();

                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                addOn();

                //  установка таймера
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();
            }
            
        }

        private void buttonAddOff_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void addOn() {
            m_sqlCmd.CommandText = "INSERT INTO tb_working(time_on,time_off) VALUES('" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "','" + DateTime.Now.ToString("yyyy - MM - ddTHH:mm: ss") + "')";
            m_sqlCmd.ExecuteNonQuery();
            m_sqlCmd.CommandText = "SELECT last_insert_rowid() FROM tb_working ";
            id_working = Convert.ToInt32(m_sqlCmd.ExecuteScalar());
        }


        void addOff()
        {
            m_sqlCmd.CommandText = "UPDATE tb_working SET time_off='" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' WHERE id=" + id_working.ToString();
            m_sqlCmd.ExecuteNonQuery();
        }

        void CreateDatabase() {
            SQLiteConnection.CreateFile(dbFileName);
            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS tb_working (id INTEGER PRIMARY KEY AUTOINCREMENT, time_on TEXT, time_off TEXT)";
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS tb_software (id INTEGER PRIMARY KEY AUTOINCREMENT,id_working INTEGER, name_software TEXT INTEGER, time_on TEXT, time_off TEXT, FOREIGN KEY (id_working) REFERENCES tb_working(id))";
                m_sqlCmd.ExecuteNonQuery();

                windows2.Title = "Connected";
            }
            catch (SQLiteException ex)
            {
                windows2.Title = "Disconnected";
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
