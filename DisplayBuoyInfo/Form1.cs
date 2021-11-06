using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayBuoyInfo
{
    public partial class Form1 : Form
    {

        BuoyInfoAccessor _buoyData;

        public Form1()
        {
            _buoyData = new BuoyInfoAccessor();
            InitializeComponent();
        }

        public Form1(BuoyInfoAccessor buoyInfoAccessor)
        {
            _buoyData = buoyInfoAccessor;
            InitializeComponent();
        }



        private void setDataOnForm()
        {

            timeZoneLabel.Text = _buoyData.TimeZoneName();
            dateBox.Text = _buoyData.Date();
            windDirectionBox.Text = _buoyData.WindDirection();
            windSpeedBox.Text = _buoyData.WindSpeed(knotsCheckBox.Checked ? BuoyInfoAccessor.KNOTS : BuoyInfoAccessor.METERS_PER_SECOND);
            gustBox.Text = _buoyData.Gust(knotsCheckBox.Checked ? BuoyInfoAccessor.KNOTS : BuoyInfoAccessor.METERS_PER_SECOND);
            waveHeightBox.Text = _buoyData.WaveHeight(BuoyInfoAccessor.METERS);
            AirTempBox.Text = _buoyData.AirTemp(fahrenheitCheckBox.Checked ? BuoyInfoAccessor.FAHRENHEIT : BuoyInfoAccessor.CELSIUS);
            seaSurfaceTempBox.Text = _buoyData.SeaSurfaceTemp(fahrenheitCheckBox.Checked ? BuoyInfoAccessor.FAHRENHEIT : BuoyInfoAccessor.CELSIUS);

        }

        private void resetDataOnForm()
        {
            foreach (var e in new List<Control>() { timeZoneLabel, dateBox, windDirectionBox, windSpeedBox, gustBox, waveHeightBox, AirTempBox, seaSurfaceTempBox })
            {
                e.Text = null;
            }
        }




        private async void getDataButton_Click(object sender, EventArgs e)
        {
 
            await _buoyData.DownloadFileAsync(buoyIdentifier.Text);

            _buoyData.FetchData(buoyIdentifier.Text);

            if (_buoyData.HasData)
            {
                setDataOnForm();
                MessageBox.Show(String.Format("Retrieved data for buoy {0}.", buoyIdentifier.Text));

            }
            else
            {
                MessageBox.Show("No information was retrieved. This could be because you are not connected to the internet or the information was not able to be downloaded.");
            }
        }





        


        

        private void nextButton_Click(object sender, EventArgs e)
        {

            if (_buoyData.HasData)
            {

                // If next data item returns false, we have reached the last record
                if (_buoyData.NextDataItem()){
                    Console.WriteLine("Next!");
                    setDataOnForm();
                } else {
                    MessageBox.Show("End of data.");
                }

            } else
            {
               MessageBox.Show("No data has been retrieved yet");
            }
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (_buoyData.HasData)
            {
                if (_buoyData.PreviousDataItem()) {
                     Console.WriteLine("Previous!");
                    setDataOnForm();
                }
                else
                {
                    MessageBox.Show("Reached beginning of data.");
                }
            }
            else
            {
                MessageBox.Show("No data has been retrieved yet");
            }
        }


        private void fahrenheitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_buoyData.HasData) setDataOnForm();
           
        }

        private void knotsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_buoyData.HasData) setDataOnForm();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["buoyIdentifiers"] != null) {
                buoyIdentifier.DataSource = ConfigurationManager.AppSettings["buoyIdentifiers"].Split(',');
            } else {
                buoyIdentifier.DataSource = new List<string>();
            }
        }

        private void buoyIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (buoyIdentifier != null && buoyIdentifier.Text != null)
            {

                _buoyData.FetchData(buoyIdentifier.Text); // read the buoy data from the file if the file exists

                // set the data on the form if it has been retrieved already or reset form fields to empty
                if (_buoyData.HasData)
                {
                    setDataOnForm();
                } else
                {
                    resetDataOnForm();
                }
            }

        }
    }
}
