using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Astrodon.Reports.Calendar
{
    public class CalendarReport
    {
        public void DrawCalendar(Graphics gr, RectangleF bounds,  DateTime first_of_month, Dictionary<int,string> date_data)
        {
            // Make the rows and columns as big as possible.
            float col_wid = bounds.Width / 7f;

            // See how many weeks we will need.
            int num_rows = NumberOfWeekRows(first_of_month);

            // Add an extra row for the month and year at the top.
            num_rows++;

            // Calculate the row height.
            float row_hgt = bounds.Height / (float)num_rows;

            // Draw the month and year.
            float x = bounds.X;
            float y = bounds.Y;
            RectangleF rectf = new RectangleF(x, y, bounds.Width, row_hgt / 2f);
            DrawMonthAndYear(gr, rectf, first_of_month);
            y += row_hgt / 2f;

            // Draw the day names.
            DrawWeekdayNames(gr, x, y, col_wid, row_hgt / 2f);
            y += row_hgt / 2f;

            // Draw the date cells.
            DrawDateData(first_of_month, date_data, gr, x, y, col_wid, row_hgt);

            // Outline the calendar.
            gr.DrawRectangle(Pens.Black,
                bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }


        // Return the number of week rows needed by this month.
        private int NumberOfWeekRows(DateTime first_of_month)
        {
            // Get the number of days in the month.
            int num_days = DateTime.DaysInMonth(
                first_of_month.Year, first_of_month.Month);

            // Add the column number for the first day of the month.
            num_days += DateColumn(first_of_month);

            // Divide by 7 and round up.
            return (int)Math.Ceiling(num_days / 7f);
        }

        private int DateColumn(DateTime date)
        {
            int col =
                (int)date.DayOfWeek -
                (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (col < 0) col += 7;
            return col;
        }

        // Draw the month and year.
        private void DrawMonthAndYear(Graphics gr, RectangleF rectf, DateTime date)
        {
            using (StringFormat sf = new StringFormat())
            {
                // Center the text.
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                string[] month_names =
                    CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                string title = month_names[date.Month - 1] +
                    " " + date.Year.ToString();

                // Find the biggest font that will fit.
                int font_size = FindFontSize(gr, rectf, "Times New Roman", title);

                // Draw the text.
                gr.FillRectangle(Brushes.LightBlue, rectf);
                using (Font font = new Font("Times New Roman", font_size))
                {
                    gr.DrawString(title, font, Brushes.Blue, rectf, sf);
                }
            }
        }

        // Draw the weekday names.
        private void DrawWeekdayNames(Graphics gr, float x, float y, float col_wid, float hgt)
        {
            // Find the widest day name.
            float max_wid = 0;
            string[] day_names =
                CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
            string widest_name = day_names[0];
            using (Font font = new Font("Times New Roman", 10))
            {
                foreach (string name in day_names)
                {
                    SizeF size = gr.MeasureString(name, font);
                    if (max_wid < size.Width)
                    {
                        max_wid = size.Width;
                        widest_name = name;
                    }
                }
            }

            // Find the biggest font size that will fit.
            RectangleF rectf = new RectangleF(x, y, col_wid, hgt);
            int font_size = FindFontSize(gr, rectf, "Times New Roman", widest_name);

            // Draw the day names.
            using (Font font = new Font("Times New Roman", font_size))
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    int index = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                    for (int i = 0; i < 7; i++)
                    {
                        gr.FillRectangle(Brushes.LightBlue, rectf);
                        gr.DrawString(day_names[index], font, Brushes.Blue, rectf, sf);
                        index = (index + 1) % 7;
                        rectf.X += col_wid;
                    }
                }
            }
        }

        // Find the largest integer font size that will fit in the given space.
        private int FindFontSize(Graphics gr, RectangleF rectf, string font_name, string text)
        {
            for (int font_size = 5; ; font_size++)
            {
                using (Font font = new Font(font_name, font_size))
                {
                    SizeF text_size = gr.MeasureString(text, font);
                    if ((text_size.Width > rectf.Width) ||
                        (text_size.Height > rectf.Height))
                        return font_size - 1;
                }
            }
        }


        private void DrawDateData(DateTime first_of_month, Dictionary<int,string> date_data,   Graphics gr, float x, float y, float col_wid, float row_hgt)
        {
            // Let date numbers occupy the upper quarter
            // and left third of the date box.
            RectangleF date_rectf =
                new RectangleF(x, y, col_wid / 3f, row_hgt / 4f);

            // The date data goes below the date rectangle.
            RectangleF data_rectf =
                new RectangleF(x, y, col_wid, row_hgt * 0.75f);

            // See how big we can make the font.
            int font_size = FindFontSize(gr, date_rectf, "Times New Roman", "30");

            // Get the column number for the first day of the month.
            int col = DateColumn(first_of_month);

            // Draw the dates.
            using (Font number_font = new Font("Times New Roman", font_size))
            {
                using (Font data_font = new Font("Times New Roman", font_size * 0.75f))
                {
                    using (StringFormat ul_sf = new StringFormat())
                    {
                        ul_sf.Alignment = StringAlignment.Near;
                        ul_sf.LineAlignment = StringAlignment.Near;
                        ul_sf.Trimming = StringTrimming.EllipsisWord;
                        ul_sf.FormatFlags = StringFormatFlags.LineLimit;

                        int num_days = DateTime.DaysInMonth(
                            first_of_month.Year, first_of_month.Month);
                        for (int day_num = 0; day_num < num_days; day_num++)
                        {
                            // Outline the cell.
                            RectangleF cell_rectf = new RectangleF(
                                x + col * col_wid, y, col_wid, row_hgt);
                            gr.DrawRectangle(Pens.Black,
                                cell_rectf.X, cell_rectf.Y,
                                cell_rectf.Width, cell_rectf.Height);

                            // Draw the date.
                            date_rectf.X = cell_rectf.X;
                            date_rectf.Y = cell_rectf.Y;
                            gr.DrawString((day_num + 1).ToString(),
                                number_font, Brushes.Blue, date_rectf, ul_sf);

                            // Draw the data.
                            data_rectf.X = x + col * col_wid;
                            data_rectf.Y = y + row_hgt * 0.25f;
                            if (date_data.ContainsKey(day_num + 1))
                            {
                                gr.DrawString(date_data[day_num + 1], data_font, Brushes.Black, data_rectf, ul_sf);
                            }
                            // Move to the next cell.
                            col = (col + 1) % 7;
                            if (col == 0) y += row_hgt;
                        }
                    }
                }
            }
        }


    }
}
