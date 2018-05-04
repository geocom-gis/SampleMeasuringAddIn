using System.Windows.Controls;
using GeoAPI.Geometries;
using GEONIS.Core.Logging;
using GEONIS.gear.AddIn.MapTools;
using GEONIS.Runtime.Core;
using GEONIS.Runtime.Core.Features;
using GEONIS.Ui.Wpf.ApplicationFramework.Messages;
using SampleMeasuringAddIn.Services;
using SampleMeasuringAddIn.View;
using SampleMeasuringAddIn.ViewModel;

namespace SampleMeasuringAddIn
{
    /// <summary>
    /// MapTool class for the SampleMeasuringMapTool .
    /// </summary>
	public class SampleMeasuringMapTool : GMapToolBase
    {
        #region Public Properties

        /// <inheritdoc />
        public override bool IsActive => _viewModel.IsActive;
        
        /// <inheritdoc />
        public override Control ToolControl => _view;

        #endregion

        #region Constants and Fields

        private readonly Control _view;

        private readonly SampleMeasuringMapToolViewModel _viewModel;

        #endregion

        #region Constructors and Destructorsd

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleMeasuringMapTool" /> class.
        /// </summary>
        public SampleMeasuringMapTool(SampleMeasuringDrawingService drawingService, IMessageContext messageContext, IGLogger logger)
        {
            logger.Error("i'm running inside the tool");
            _view = new SampleMeasuringMapToolView();
            _viewModel = new SampleMeasuringMapToolViewModel(drawingService, messageContext, logger);
            _view.DataContext = _viewModel;
            _viewModel.PropertyChanged += OnToolViewModelActivationChanged;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override void Activate()
        {
            _viewModel.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            _viewModel.Deactivate();
        }

        /// <inheritdoc />
        public override void ExecuteMapClick(IPoint point)
        {
            _viewModel.OnMapClicked(point);
        }

        /// <inheritdoc />
        public override void RegisterWithMap(IGMapModel map)
        {
            _viewModel.RegisterMap(map);
        }

        /// <inheritDoc />
        public override bool SupportsObject(GeomObject geomObject)
        {
            return false;
        }

        #endregion
    }
}
