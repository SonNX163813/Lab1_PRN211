﻿using AutomobileLibrary.Repository;
using AutomobileLibrary.BussinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AutomobileWinApp
{
    public partial class frmCarManagement : Form
    {
        ICarRepository carRepository = new CarRepository();
        //create a data source
        BindingSource source;
        //------------------------------------------------------------------
        public frmCarManagement()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbReleaseYear_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void frmCarManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            //register this event to open the  frmCardetails from that perform updating 
            dgvCarList.CellDoubleClick += DgvCarList_CellDoubleClick;
        }
        //--------------------------------------------------------------------------
        private void DgvCarList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCarDetails frmCarDetails = new frmCarDetails
            {
                Text = "Update car",
                InsertOrUpdate = true,
                CarInfo = GetCarObject(),
                CarRepository = carRepository
            };
            if (frmCarDetails.ShowDialog() == DialogResult.OK)
            {
                LoadCarList();
                //set focus car updated
                source.Position = source.Count - 1;
            }
        }
        //Clear text on TextBoxes
        private void ClearText()
        {
            txtCarID.Text = string.Empty;
            txtCarName.Text = string.Empty;
            txtManufacturer.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtReleaseYear.Text = string.Empty;

        }
        //------------------------------------------------
        private Car GetCarObject()
        {
            Car car = null;
            try
            {
                car = new Car
                {
                    CarID = int.Parse(txtCarID.Text),
                    CarName = txtCarName.Text,
                    Manufacturer = txtManufacturer.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    ReleasedYear = int.Parse(txtReleaseYear.Text)
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get car");
            }
            return car;
        }
        //end getObject
        public void LoadCarList()
        {
            var cars = carRepository.GetCars();
            try
            {
                source = new BindingSource();
                source.DataSource = cars;

                txtCarID.DataBindings.Clear();
                txtCarName.DataBindings.Clear();
                txtManufacturer.DataBindings.Clear();
                txtPrice.DataBindings.Clear();
                txtReleaseYear.DataBindings.Clear();

                txtCarID.DataBindings.Add("Text", source, "CarID");
                txtCarName.DataBindings.Add("Text", source, "CarName");
                txtManufacturer.DataBindings.Add("Text", source, "Manufacturer");
                txtPrice.DataBindings.Add("Text", source, "Price");
                txtReleaseYear.DataBindings.Add("Text", source, "ReleasedYear");

                dgvCarList.DataSource = null;
                dgvCarList.DataSource = source;
                if (cars.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load car list");
            }
        }
        //end loadcarlist
        //----------------------------------------------------
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadCarList();
        }
        //end btnLoad_Click
        //-----------------------------------------
        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmCarDetails frmCarDetails = new frmCarDetails
            {
                Text = "Add car",
                InsertOrUpdate = false,
                CarRepository = carRepository
            };
            if (frmCarDetails.ShowDialog() == DialogResult.OK)
            {
                LoadCarList();
                //Set focus car insertd
                source.Position = source.Count - 1;
            }
        }
        //End btnNew_Click
        //-----------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var car = GetCarObject();
                carRepository.DeleteCar(car.CarID);
                LoadCarList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a car");
            }
        }
        //End btnDelete_Click
    } //End form
}
