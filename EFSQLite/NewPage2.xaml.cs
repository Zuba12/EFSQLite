using EFSQLite.Data;
using EFSQLite.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;

namespace EFSQLite;

public partial class NewPage2 : ContentPage
{
    MyContext _context;
    Faktury2 _faktury2;

    public NewPage2()
    {
        _faktury2 = new();
        _context = new MyContext();
        InitializeComponent();
        lst.ItemsSource = _faktury2.Fakturies.ToList(); // p�ipojen� zdroje dat k ListView
        QuestPDF.Settings.License = LicenseType.Community;
        forName.ItemsSource = _context.Students.ToList();
    }

    private void SaveStudent(object sender, EventArgs e)
    {
        if (forName != null)
        {
            Faktury newStudent = new()
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
                DatumVystaveni = datumVystaveni.Date.ToShortDateString(),
                DatumSplatnosti = datumSplatnosti.Date.ToShortDateString(),
            };


            _faktury2.Add(newStudent); // p�id� z�znam do Data Setu
            _faktury2.SaveChanges(); // ulo�� zm�ny do datab�ze !!!!!!
            refresh();
        
        }
        else
        {
            DisplayAlert("Chybka", "Vypl�te povinn� pole", "Ok");
        }
    }
    
    private void Smazat(object sender, EventArgs e)
    {
        Faktury keSmazani = lst.SelectedItem as Faktury;
        if (keSmazani != null)
        {
            _faktury2.Fakturies.Remove(keSmazani); // odebr�n� studenta z data setu
            _faktury2.SaveChanges(); // ulo�� zm�ny do datab�ze
            refresh();
        }
    }

    private async void PDF(object sender, EventArgs e)
    {
        Faktury keSmazani = lst.SelectedItem as Faktury;

        string accountNumber = keSmazani.AccountNumber;
        int celkcena = Int32.Parse(keSmazani.Price) * Int32.Parse(keSmazani.PocetKusu);

        string paymentString = $"SPD*1.0*ACC:{accountNumber}";

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(paymentString, QRCodeGenerator.ECCLevel.L);

        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        string imageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qrcode.png");
        File.WriteAllBytes(imageFilePath, qrCodeBytes);
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Prijate.pdf");
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(QuestPDF.Helpers.Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Z�hlav� faktury
                page.Header()
                    .AlignCenter()
                    .Text("Faktura")
                    .SemiBold().FontSize(24).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);


                // Informace o z�kazn�kovi
                page.Content()
                    .Background(QuestPDF.Helpers.Colors.Grey.Lighten2)
                    .Padding(12)

                    .Text(text =>
                    {
                        text.Span("Dodavatel: ").Bold();
                        text.Span($"{keSmazani.Name}\n");

                        text.Span("Jm�no a P��jmen�: ").Bold();
                        text.Span($"{keSmazani.SurName}\n");

                        text.Span("Adresa: ").Bold();
                        text.Span($"{keSmazani.Address}\n");

                        text.Span("PS�: ").Bold();
                        text.Span($"{keSmazani.PSC}\n");

                        text.Span("Email: ").Bold();
                        text.Span($"{keSmazani.Email}\n");

                        text.Span("Telefonn� ��slo: ").Bold();
                        text.Span($"{keSmazani.Number}\n");

                        text.Span("Atribut: ").Bold();
                        text.Span($"{keSmazani.Atrribute}\n");

                        text.Span("Cena: ").Bold();
                        text.Span($"{keSmazani.Price}\n");

                        text.Span("Zp�sob platby: ").Bold();
                        text.Span($"{keSmazani.Zpusob}\n");

                        text.Span("��slo ��tu: ").Bold();
                        text.Span($"{keSmazani.AccountNumber}\n");

                        text.Span("Po�et kus�: ").Bold();
                        text.Span($"{keSmazani.PocetKusu}\n");

                        text.Span("datum vystaven�: ").Bold();
                        text.Span($"{keSmazani.DatumVystaveni}\n");

                        text.Span("datem splatnosti: ").Bold();
                        text.Span($"{keSmazani.DatumSplatnosti}\n");




                        // Celkov� cena
                        double cena;
                        if (double.TryParse(keSmazani.Price, out double parsedPrice) && double.TryParse(keSmazani.PocetKusu, out double parsedQuantity))
                        {
                            cena = parsedPrice * parsedQuantity;
                            text.Span($"Celkov� cena: ").Bold().FontColor(QuestPDF.Helpers.Colors.Red.Darken1);
                            text.Span($"{cena}\n");
                        }
                        else
                        {
                            text.Span("Neplatn� �daje pro v�po�et celkov� ceny.");
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
        lst.ItemsSource = _faktury2.Fakturies.ToList();
    }
}