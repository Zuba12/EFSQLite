using EFSQLite.Data;
using EFSQLite.Models;
using Microsoft.Maui.Controls;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EFSQLite;

public partial class NewPage2 : ContentPage
{
    MyContext _context;
    Faktury2 _faktury2;

    public NewPage2()
    {
        _faktury2 = new();
        InitializeComponent();
        lst.ItemsSource = _faktury2.Fakturies.ToList(); // p�ipojen� zdroje dat k ListView
        QuestPDF.Settings.License = LicenseType.Community;
        forName.ItemsSource = _context.Students.ToList();
    }

    private void SaveStudent(object sender, EventArgs e)
    {
        Faktury newStudent = new()
        {
            Email = forEmail.Text,
            Number = forNumber.Text,
            Address = forAddress.Text,
            Product = forProduct.Text,
            Price = forPrice.Text,
            Zpusob = forZpusob.Text,
            AccountNumber = forAccountNumber.Text,
            //Surname = forSurname.Text 
        };

        _faktury2.Add(newStudent); // p�id� z�znam do Data Setu
        _faktury2.SaveChanges(); // ulo�� zm�ny do datab�ze !!!!!!
        refresh();
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
        lst.ItemsSource = _faktury2.Fakturies.ToList();
    }
}