using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GeoAPI.Geometries;
using GEONIS.Core.Language;
using GEONIS.Runtime.Core;
using GEONIS.Runtime.Core.Drawing;
using GEONIS.Runtime.Core.MapLayers;
using GEONIS.Runtime.Core.Utils;
using NetTopologySuite.Geometries;

namespace SampleMeasuringAddIn.Services
{
    /// <summary>
    /// A drawing service that manages the drawn vertices and edges on a layer
    /// </summary>
    public class SampleMeasuringDrawingService : ViewModelBase
    {
        #region fields
        private IGDrawingLayer _drawingLayer;
        private ICommand _labelUndoCommand;
        private double _totalLength;
        private readonly IGMapModel _mapModel;
        private readonly List<IPoint> _vertices;
        private readonly Symbol _vertexSymbol = new PointSymbol(Colors.DarkRed, 10);
        private readonly Symbol _edgeSymbole = new LineSymbol(Colors.DarkRed, 5, LineStyle.SOLID);
        private readonly FontSymbol _lineFontSymbol = new FontSymbol(Colors.Black, 12);
        private readonly FontSymbol _totalFontSymbol = new FontSymbol(Colors.Black, 14);

        #endregion

        internal const string DRAWING_LAYER_ID = "measuringDrawingLayer";

        private const string TotalLabel = "Total length";

        private IGDrawingLayer DrawingLayer
        {
            get
            {
                if (_drawingLayer == null)
                {
                    _drawingLayer = _mapModel.GetDrawingLayer(DRAWING_LAYER_ID);
                }

                return _drawingLayer;
            }
        }

        public SampleMeasuringDrawingService(IGMapModel mapModel, IGLanguageTranslator translator)
        {
            _mapModel = mapModel;
            _vertices = new List<IPoint>();
            _labelUndoCommand = null;
            _totalLength = 0d;

            _totalFontSymbol.Bold = true;
            _lineFontSymbol.Bold = true;
        }

        private IPoint FindLastVertex(int offset = 0)
        {
            if (_vertices.Count > offset)
            {
                return _vertices[_vertices.Count - (1 + offset)];
            }
            return null;
        }

        internal void AddVertex(IPoint point)
        {
            if (_vertices.Contains(point))
                return;

            _vertices.Add(point);

            DrawingLayer.DrawGeometry(point, _vertexSymbol);

            if (_vertices.Count > 1)
            {
                IPoint lastVertex = FindLastVertex(1);
                ILineString edge = CreateEdge(point, lastVertex);
                double edgeLength = Math.Round(edge.GetLength(), 2);

                _totalLength += edgeLength;

                DrawingLayer.DrawGeometry(edge, _edgeSymbole);

                IPoint txtPoint = GGeometryUtils.GetCenterPointOnLine(point, lastVertex);
                double angle = GGeometryUtils.CalculateAngleForText(point, lastVertex);

                _lineFontSymbol.Angle = angle;
                _lineFontSymbol.VerticalAlignment = VerticalAlignment.BOTTOM;
                _lineFontSymbol.XOffset = 3;
                _lineFontSymbol.YOffset = 3;

                string unitLetter = GGeometryUtils.GetMeasureUnitsFromMapPoint(point);
                string edgeLabel = $"{edgeLength} {unitLetter}";

                DrawingLayer.DrawText(txtPoint, _lineFontSymbol, edgeLabel);

                RedrawTotalLabel();
            }
        }

        private ILineString CreateEdge(IPoint vertex1, IPoint vertex2)
        {
            Coordinate[] coordinates = {
                vertex1.Coordinate,
                vertex2.Coordinate
            };
            return new LineString(coordinates);
        }

        private void RedrawTotalLabel()
        {
            if (_labelUndoCommand != null)
            {
                _labelUndoCommand.Execute(null);
                _labelUndoCommand = null;
            }

            IPoint last = FindLastVertex();
            IPoint secondLast = FindLastVertex(1);

            if (secondLast != null)
            {
                FontSymbol labelSymbol = _totalFontSymbol;

                double deltaX = last.X - secondLast.X;
                double deltaY = last.Y - secondLast.Y;
                double vLength = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                double vDx = 2 * deltaX / vLength;
                double vDy = deltaY / vLength;

                labelSymbol.HorizontalAlignment = deltaX < 0 ? HorizontalAlignment.RIGHT : HorizontalAlignment.LEFT;
                labelSymbol.XOffset = ((PointSymbol) _vertexSymbol).Size / 2;
                labelSymbol.XOffset *= deltaX < 0 ? -1 : 1;

                string unitLetter = GGeometryUtils.GetMeasureUnitsFromMapPoint(last);

                string totalLabel = $"{TotalLabel}: {_totalLength} {unitLetter}";

                _labelUndoCommand = DrawingLayer.DrawText(last.ShiftPoint(vDx, vDy), labelSymbol, totalLabel);
            }
        }

        internal void Clear()
        {
            _totalLength = 0;
            _labelUndoCommand = null;
            _vertices.Clear();
            _drawingLayer.ClearLayer();
        }

    }
}
