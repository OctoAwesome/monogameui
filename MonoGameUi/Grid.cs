using System;
using System.Collections.Generic;
using System.Linq;
using engenious;

namespace MonoGameUi
{
    public class Grid : Control
    {
        private List<CellMapping> cellMapping = new List<CellMapping>();

        public List<ColumnDefinition> Columns { get; private set; }

        public List<RowDefinition> Rows { get; private set; }

        public Grid(BaseScreenComponent manager, string style = "") :
            base(manager, style)
        {
            Columns = new List<ColumnDefinition>();
            Rows = new List<RowDefinition>();
            ApplySkin(typeof(Grid));
        }

        public void AddControl(Control control, int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            if (column < 0 || column > Columns.Count - 1)
                throw new ArgumentOutOfRangeException("column");

            if (row < 0 || row > Rows.Count - 1)
                throw new ArgumentOutOfRangeException("row");

            if (columnSpan < 1)
                throw new ArgumentOutOfRangeException("columnSpan");

            if (rowSpan < 1)
                throw new ArgumentOutOfRangeException("rowSpan");

            var mapping = new CellMapping(control);
            for (int x = 0; x < columnSpan; x++)
            {
                int colIndex = column + x;
                if (colIndex < 0) continue;
                if (colIndex >= Columns.Count) continue;
                mapping.Columns.Add(Columns[colIndex]);
            }

            for (int y = 0; y < rowSpan; y++)
            {
                int rowIndex = row + y;
                if (rowIndex < 0) continue;
                if (rowIndex >= Rows.Count) continue;
                mapping.Rows.Add(Rows[rowIndex]);
            }

            cellMapping.Add(mapping);
            Children.Add(control);
        }

        public override Point GetExpectedSize(Point available)
        {
            Point result = GetMinClientSize(available);
            Point client = GetMaxClientSize(available);

            // Restliche Controls
            foreach (var mapping in cellMapping)
            {
                Point cell = new Point(
                    mapping.Columns.All(c => c.ResizeMode == ResizeMode.Fixed) ? mapping.Columns.Sum(c => c.Width) : client.X,
                    mapping.Rows.All(r => r.ResizeMode == ResizeMode.Fixed) ? mapping.Rows.Sum(r => r.Height) : client.Y);
                mapping.ExpectedSize = mapping.Control.GetExpectedSize(cell);
            }

            #region Columns

            // Statische Spalten ermitteln
            int totalWidth = 0;
            foreach (var column in Columns.Where(c => c.ResizeMode == ResizeMode.Fixed))
            {
                column.ExpectedWidth = column.Width;
                totalWidth += column.ExpectedWidth;
            }

            // Automatische Spalten ermitteln
            foreach (var column in Columns.Where(c => c.ResizeMode == ResizeMode.Auto))
            {
                int width = column.MinWidth.HasValue ? column.MinWidth.Value : 0;
                foreach (var mapping in cellMapping.Where(m => m.Columns.Contains(column)))
                {
                    int mapWidth = mapping.Columns.Where(c => c.ResizeMode == ResizeMode.Fixed).Sum(c => c.ExpectedWidth);
                    int autoCount = mapping.Columns.Count(c => c.ResizeMode == ResizeMode.Auto);
                    width = Math.Max(width, (mapping.ExpectedSize.X - mapWidth) / autoCount);
                }
                if (column.MaxWidth.HasValue)
                    width = Math.Min(width, column.MaxWidth.Value);
                column.ExpectedWidth = width;
                totalWidth += column.ExpectedWidth;
            }

            // Anteilige Spalten ermitteln
            int partsX = Columns.Where(c => c.ResizeMode == ResizeMode.Parts).Sum(c => c.Width);
            if (partsX > 0)
            {
                int partX = (client.X - totalWidth) / partsX;
                foreach (var column in Columns.Where(c => c.ResizeMode == ResizeMode.Parts))
                {
                    column.ExpectedWidth = partX * column.Width;
                    totalWidth += column.ExpectedWidth;
                }
            }

            #endregion

            #region Rows

            // Statische Zeilen ermitteln
            int totalHeight = 0;
            foreach (var row in Rows.Where(r => r.ResizeMode == ResizeMode.Fixed))
            {
                row.ExpectedHeight = row.Height;
                totalHeight += row.ExpectedHeight;
            }

            // Automatische Spalten ermitteln
            foreach (var row in Rows.Where(r => r.ResizeMode == ResizeMode.Auto))
            {
                int height = row.MinHeight.HasValue ? row.MinHeight.Value : 0;
                foreach (var mapping in cellMapping.Where(m => m.Rows.Contains(row)))
                {
                    int mapHeight = mapping.Rows.Where(r => r.ResizeMode == ResizeMode.Fixed).Sum(r => r.ExpectedHeight);
                    int autoCount = mapping.Rows.Count(r => r.ResizeMode == ResizeMode.Auto);
                    height = Math.Max(height, (mapping.ExpectedSize.Y - mapHeight) / autoCount);
                }
                if (row.MaxHeight.HasValue)
                    height = Math.Min(height, row.MaxHeight.Value);
                row.ExpectedHeight = height;
                totalHeight += row.ExpectedHeight;
            }

            // Anteilige Spalten ermitteln
            int partsY = Rows.Where(c => c.ResizeMode == ResizeMode.Parts).Sum(r => r.Height);
            if (partsY > 0)
            {
                int partY = (client.Y - totalHeight) / partsY;
                foreach (var row in Rows.Where(r => r.ResizeMode == ResizeMode.Parts))
                {
                    row.ExpectedHeight = partY * row.Height;
                    totalHeight += row.ExpectedHeight;
                }
            }

            #endregion

            result = new Point(Math.Max(result.X, totalWidth), Math.Max(result.Y, totalHeight));

            return result + Borders;
        }

        public override void SetActualSize(Point available)
        {
            Point minSize = GetExpectedSize(available);
            SetDimension(minSize, available);

            #region Reorg Cols & Rows

            int offsetX = 0;
            foreach (var column in Columns)
            {
                column.ActualOffset = offsetX;
                column.ActualWidth = column.ExpectedWidth;
                offsetX += column.ActualWidth;
            }

            int offsetY = 0;
            foreach (var row in Rows)
            {
                row.ActualOffset = offsetY;
                row.ActualHeight = row.ExpectedHeight;
                offsetY += row.ActualHeight;
            }

            #endregion

            #region Set Controls

            foreach (var mapping in cellMapping)
            {
                Point cellOffset = new Point(mapping.Columns[0].ActualOffset, mapping.Rows[0].ActualOffset);
                Point cellSize = new Point(mapping.Columns.Sum(c => c.ActualWidth), mapping.Rows.Sum(r => r.ActualHeight));

                mapping.Control.SetActualSize(cellSize);
                mapping.Control.ActualPosition += cellOffset;
            }

            #endregion
        }


        /// <summary>
        /// Interne Klasse zur Haltung der Cell-Mappings
        /// </summary>
        private class CellMapping
        {
            /// <summary>
            /// Referenz auf das entsprechende Control
            /// </summary>
            public Control Control { get; private set; }

            /// <summary>
            /// Auflistung der betroffenen Columns
            /// </summary>
            public List<ColumnDefinition> Columns { get; private set; }

            /// <summary>
            /// Auflistung der betroffenen Rows
            /// </summary>
            public List<RowDefinition> Rows { get; private set; }

            /// <summary>
            /// Erwartete Control-Größe
            /// </summary>
            public Point ExpectedSize { get; set; }

            public CellMapping(Control control)
            {
                Columns = new List<ColumnDefinition>();
                Rows = new List<RowDefinition>();
                Control = control;
            }
        }
    }

    public class ColumnDefinition
    {
        public ResizeMode ResizeMode { get; set; }

        public int? MinWidth { get; set; }

        public int Width { get; set; }

        public int? MaxWidth { get; set; }

        public int ActualOffset { get; set; }

        public int ActualWidth { get; set; }

        public int ExpectedWidth { get; set; }
    }

    public class RowDefinition
    {
        public ResizeMode ResizeMode { get; set; }

        public int? MinHeight { get; set; }

        public int Height { get; set; }

        public int? MaxHeight { get; set; }

        public int ActualOffset { get; set; }

        public int ActualHeight { get; set; }

        public int ExpectedHeight { get; set; }
    }

    public enum ResizeMode
    {
        Fixed,
        Auto,
        Parts
    }
}
