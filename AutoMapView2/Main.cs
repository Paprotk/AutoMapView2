using Sims3.SimIFace;
using Sims3.UI;
using OneShotFunctionTask = Sims3.Gameplay.OneShotFunctionTask;

namespace Arro.AutoMapView2
{
	public class Main
	{
		[Tunable]
        public static bool kInstantiator = false;

		private static SceneMgrWindow _sceneMgrWindow;
		private static bool _mapViewDisabledRecently;
		
		static Main()
		{
			CameraController.OnCameraAtMaxZoomCallback += OnCameraAtMaxZoom;
			CameraController.OnCameraMapViewEnabledCallback += OnMapViewEnabled;
		}
		
		private static void OnCameraAtMaxZoom(bool atMaxZoom)
		{
			if (atMaxZoom && !_mapViewDisabledRecently)
			{
				Responder.Instance.CameraModel.ToggleMapView();
			}
		}
		
		private static void OnMapViewEnabled(bool enabled)
		{
			if (enabled)
			{
				_sceneMgrWindow = UIManager.GetSceneWindow();
				_sceneMgrWindow.MouseWheel -= OnMouseWheel;
				_sceneMgrWindow.MouseWheel += OnMouseWheel;
				return;
			}
			_mapViewDisabledRecently = true;
			Simulator.AddObject(new OneShotFunctionTask(() => 
			{
				_mapViewDisabledRecently = false;
			}, StopWatch.TickStyles.Seconds, 1.1f));
			
		}

		private static void OnMouseWheel(WindowBase sender, UIMouseEventArgs eventArgs)
		{
			int wheelDelta = eventArgs.MouseWheelDelta;
			ScenePickArgs pickArgs = _sceneMgrWindow.GetPickArgs();
			Vector3 worldPos = pickArgs.mWorldPos;
			if (wheelDelta > 0)
			{
				eventArgs.Handled = true;
				CameraController.RequestLerpToTarget(worldPos, 1f, false);
				Responder.Instance.CameraModel.ToggleMapView();
				_sceneMgrWindow.MouseWheel -= OnMouseWheel;
			}
		}
	}
}