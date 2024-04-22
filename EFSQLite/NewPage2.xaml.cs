using EFSQLite.Data;
using EFSQLite.Models;
using Microsoft.Maui.Controls;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EFSQLite;

public partial class NewPage2 : ContentPage
{
    Faktury2 _context;

    public NewPage2()
    {
        _context = new();
        InitializeComponent();
        lst.ItemsSource = _context.Students.ToList(); // pøipojení zdroje dat k ListView
        QuestPDF.Settings.License = LicenseType.Community;
    }

    private void SaveStudent(object sender, EventArgs e)
    {
        Faktury newStudent = new()
        {
            Name = forName.Text,
            Email = forEmail.Text,
            Number = forNumber.Text,
            Address = forAddress.Text,
            Product = forProduct.Text,
            Price = forPrice.Text,
            Zpusob = forZpusob.Text,
            AccountNumber = forAccountNumber.Text,
            //Surname = forSurname.Text 
        };

        _context.Add(newStudent); // pøidá záznam do Data Setu
        _context.SaveChanges(); // uloží zmìny do databáze !!!!!!
        refresh();
    }

    private void Smazat(object sender, EventArgs e)
    {
        Faktury keSmazani = lst.SelectedItem as Faktury;
        if (keSmazani != null)
        {
            _context.Students.Remove(keSmazani); // odebrání studenta z data setu
            _context.SaveChanges(); // uloží zmìny do databáze
            refresh();
        }
    }

    private async void PDF(object sender, EventArgs e)
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "invoice.pdf");
        Document.Create(container =>
        {


            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(QuestPDF.Helpers.Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Hello PDF!")
                    .SemiBold().FontSize(36).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text(Placeholders.LoremIpsum());
                        x.Item().Image(Placeholders.Image(200, 100));
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        })
.GeneratePdf(path);
    }

    void refresh()
    {
        lst.ItemsSource = null;
        lst.ItemsSource = _context.Students.ToList();
    }
}