using Sims3.SimIFace;
using Sims3.UI;

namespace Arro.AutoMapView2
{
	public class Main
	{
		[Tunable]
        public static bool kInstantiator = false;

		private static SceneMgrWindow _sceneMgrWindow;
		
		static Main()
		{
			CameraController.OnCameraAtMaxZoomCallback += OnCameraAtMaxZoom;
			CameraController.OnCameraMapViewEnabledCallback += OnMapViewEnabled;
		}
		
		private static void OnCameraAtMaxZoom(bool atMaxZoom)
		{
			if (atMaxZoom)
			{
				Responder.Instance.CameraModel.ToggleMapView();
			}
		}
		
		private static void OnMapViewEnabled(bool enabled)
		{
			_sceneMgrWindow = UIManager.GetSceneWindow();
			_sceneMgrWindow.MouseWheel -= OnMouseWheel;
			_sceneMgrWindow.MouseWheel += OnMouseWheel;
		}

		private static void OnMouseWheel(WindowBase sender, UIMouseEventArgs eventArgs)
		{
			int wheelDelta = eventArgs.MouseWheelDelta;
			ScenePickArgs pickArgs = _sceneMgrWindow.GetPickArgs();
			Vector3 worldPos = pickArgs.mWorldPos;
			if (wheelDelta > 0)
			{
				CameraController.RequestLerpToTarget(worldPos, 1f, false);
				Responder.Instance.CameraModel.ToggleMapView();
				_sceneMgrWindow.MouseWheel -= OnMouseWheel;
			}
		}
	}
}