using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Text;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;



namespace Billing.Entity
{
    public class Ejemplo : PdfPageEventHelper
    {

        
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable table = new PdfPTable(1);
                //table.WidthPercentage = 100; //PdfPTable.writeselectedrows below didn't like this
                table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]
                PdfPTable table2 = new PdfPTable(2);

                //logo
                //PdfPCell cell2 = new PdfPCell(Image.GetInstance(@"\D:\Proyecto\Omnimed-Billing\Omnimed-Billing\Billing.Web\Images\loading1.gif"));
                PdfPCell cell2 = new PdfPCell(new Phrase("\nTITLE_TEST", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD | Font.UNDERLINE)));
                cell2.Colspan = 2;
                table2.AddCell(cell2);

                //title
                cell2 = new PdfPCell(new Phrase("\nTITLE", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD | Font.UNDERLINE)));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.Colspan = 2;
                table2.AddCell(cell2);

                PdfPCell cell = new PdfPCell(table2);
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);
            }
        




    }
}
