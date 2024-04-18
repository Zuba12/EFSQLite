using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EFSQLite.Data;
using EFSQLite.Models;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace EFSQLite;



public partial class OFaktury : ContentPage
{
    Ofaktury _context;

    public OFaktury()
    {
        _context = new();
        InitializeComponent();
        lst.ItemsSource = _context.Students.ToList(); // pøipojení zdroje dat k ListView
    }  

        private void SaveStudent(object sender, EventArgs e)
        {
            Ofaktury2 newStudent = new()
            {
                Name = forName.Text,
                SurName = forSurname.Text,
                Address = forAddress.Text,
                PSC = forPSC.Text,
                Email = forEmail.Text,
                Number = forNumber.Text,
                Atrribute = forAtrribute.Text,
                Price = forPrice.Text,
                Zpusob = forZpusob.Text,
                AccountNumber = forAccountNumber.Text,
                PocetKusu = forPocetKusu.Text, 
            };

            _context.Add(newStudent); // pøidá záznam do Data Setu
            _context.SaveChanges(); // uloží zmìny do databáze !!!!!!
            refresh();
        }

        private void Smazat(object sender, EventArgs e)
        {
            Ofaktury2 keSmazani = lst.SelectedItem as Ofaktury2;
            if (keSmazani != null)
            {
                _context.Students.Remove(keSmazani); // odebrání studenta z data setu
                _context.SaveChanges(); // uloží zmìny do databáze
                refresh();
            }
        }



    private async void PDF(object sender, EventArgs e)
    {
        // Check if an item is selected in the ListView
        if (lst.SelectedItem is Ofaktury2 selectedStudent)
        {
            // Define the path for the PDF file
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "invoice.pdf");

            // Generate the PDF document
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Define the content of the page
                    page.AddText($"Name: {selectedStudent.Name}");
                    page.AddText($"Surname: {selectedStudent.SurName}");
                    page.AddText($"Address: {selectedStudent.Address}");
                    page.AddText($"Postal Code: {selectedStudent.PSC}");
                    page.AddText($"Email: {selectedStudent.Email}");
                    page.AddText($"Number: {selectedStudent.Number}");
                    page.AddText($"Attribute: {selectedStudent.Atrribute}");
                    page.AddText($"Price: {selectedStudent.Price}");
                    page.AddText($"Payment Method: {selectedStudent.Zpusob}");
                    page.AddText($"Account Number: {selectedStudent.AccountNumber}");
                    page.AddText($"Quantity: {selectedStudent.PocetKusu}");
                    page.AddText($"Issuance Date: {Datumvystavení.Date.ToString("yyyy-MM-dd")}");
                    page.AddText($"Due Date: {DatumSplacení.Date.ToString("yyyy-MM-dd")}");
                });
            }).GeneratePdf(path);

            // Open the PDF file
            Process.Start(path);
        }
        else
        {
            // Inform the user to select an item from the ListView
            await DisplayAlert("No Selection", "Please select a student from the list before generating PDF.", "OK");
        }
    }

    void refresh()
        {
            lst.ItemsSource = null;
            lst.ItemsSource = _context.Students.ToList();
        }

}