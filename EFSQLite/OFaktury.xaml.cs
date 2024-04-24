using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EFSQLite.Data;
using EFSQLite.Models;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using QRCoder;

namespace EFSQLite;



public partial class OFaktury : ContentPage
{
    MyContext _context;
    Ofaktury _ofaktury;

    public OFaktury()
    {
        _ofaktury = new();
        _context = new MyContext();
        InitializeComponent();
        lst.ItemsSource = _ofaktury.Ofakturies.ToList();  // pøipojení zdroje dat k ListView
        QuestPDF.Settings.License = LicenseType.Community;
        forName.ItemsSource = _context.Students.ToList();
    }  


        private void SaveStudent(object sender, EventArgs e)
        {
            Ofaktury2 newStudent = new()
            {
                Name = forName.SelectedItem.ToString(),
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

            _ofaktury.Add(newStudent); // pøidá záznam do Data Setu
            _ofaktury.SaveChanges(); // uloží zmìny do databáze !!!!!!
            refresh();
        }

        private void Smazat(object sender, EventArgs e)
        {
            Ofaktury2 keSmazani = lst.SelectedItem as Ofaktury2;
            if (keSmazani != null)
            {
                _ofaktury.Ofakturies.Remove(keSmazani); // odebrání studenta z data setu
                _ofaktury.SaveChanges(); // uloží zmìny do databáze
                refresh();
            }
        }



    private async void PDF(object sender, EventArgs e)
    {
        Ofaktury2 keSmazani = lst.SelectedItem as Ofaktury2;

        string accountNumber = keSmazani.AccountNumber;
        int celkcena = Int32.Parse(keSmazani.Price) * Int32.Parse(keSmazani.PocetKusu);

        string paymentString = $"SPD1.0ACC:{accountNumber}*AM:{celkcena}";

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(paymentString, QRCodeGenerator.ECCLevel.L);

        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        string imageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qrcode.png");
        File.WriteAllBytes(imageFilePath, qrCodeBytes);
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Odeslane.pdf");
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(QuestPDF.Helpers.Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Záhlaví faktury
                page.Header()
                    .AlignCenter()
                    .Text("Faktura")
                    .SemiBold().FontSize(24).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);


                // Informace o zákazníkovi
                page.Content()
                    .Background(QuestPDF.Helpers.Colors.Grey.Lighten2)
                    .Padding(12)

                    .Text(text =>
                    {
                        text.Span("Dodavatel: ").Bold();
                        text.Span($"{keSmazani.Name}\n");

                        text.Span("Jméno a Pøíjmení: ").Bold();
                        text.Span($"{keSmazani.SurName}\n");

                        text.Span("Adresa: ").Bold();
                        text.Span($"{keSmazani.Address}\n");

                        text.Span("PSÈ: ").Bold();
                        text.Span($"{keSmazani.PSC}\n");

                        text.Span("Email: ").Bold();
                        text.Span($"{keSmazani.Email}\n");

                        text.Span("Telefonní èíslo: ").Bold();
                        text.Span($"{keSmazani.Number}\n");

                        text.Span("Atribut: ").Bold();
                        text.Span($"{keSmazani.Atrribute}\n");

                        text.Span("Cena: ").Bold();
                        text.Span($"{keSmazani.Price}\n");

                        text.Span("Zpùsob platby: ").Bold();
                        text.Span($"{keSmazani.Zpusob}\n");

                        text.Span("Èíslo úètu: ").Bold();
                        text.Span($"{keSmazani.AccountNumber}\n");

                        text.Span("Poèet kusù: ").Bold();
                        text.Span($"{keSmazani.PocetKusu}\n");

                        text.Span("datum vystavení: ").Bold();
                        text.Span($"{keSmazani.DatumVystaveni}\n");

                        text.Span("datem splatnosti: ").Bold();
                        text.Span($"{keSmazani.DatumSplatnosti}\n");




                        // Celková cena
                        double cena;
                        if (double.TryParse(keSmazani.Price, out double parsedPrice) && double.TryParse(keSmazani.PocetKusu, out double parsedQuantity))
                        {
                            cena = parsedPrice * parsedQuantity;
                            text.Span($"Celková cena: ").Bold().FontColor(QuestPDF.Helpers.Colors.Red.Darken1);
                            text.Span($"{cena}\n");
                        }
                        else
                        {
                            text.Span("Neplatné údaje pro výpoèet celkové ceny.");
                        }





                    });







                page.Footer()
                    .AlignCenter()
                    .Column(Column =>
                    {
                        Column
                        .Item()
                        .Width(200)
                        .Height(200)
                        .Image(imageFilePath);
                    });
            });
        })
        .GeneratePdf(path);
    }

    void refresh()
        {
            lst.ItemsSource = null;
            lst.ItemsSource = _ofaktury.Ofakturies.ToList();
        }

}