/*
 * Copyright (c) 2017 VR Stuff
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DimensionChanger
{
	#region Events

	public static event DimensionEventHandler Changed;

	#endregion

	#region Methods

	public static void SwitchDimensions(GameObject obj, Dimension fromDimension, Dimension toDimension)
	{
		obj.layer = toDimension.layer;

        // Move over all visible children as well
        foreach(MeshRenderer childRenderer in obj.GetComponentsInChildren<MeshRenderer>())
		{
            childRenderer.gameObject.layer = toDimension.layer;
        }

		// If this is an FPS controller then make sure it goes through too.
		Transform parent = obj.transform.parent;
		if(parent != null && parent.GetComponent<CharacterController>())
		{
			parent.gameObject.layer = toDimension.layer;
		}

		// Notify listeners
		Changed?.Invoke(obj, new DimensionEventArgs(fromDimension, toDimension));
	}

	public static void SwitchCameraRender(Camera camera, int fromDimensionLayer, int toDimensionLayer, Material dimensionSkybox)
	{
		CameraExtensions.LayerCullingShow(camera, toDimensionLayer);
		CameraExtensions.LayerCullingHide(camera, fromDimensionLayer);

		if (dimensionSkybox)
		{
			var skybox = camera.GetComponent<Skybox>();
			if (skybox != null)
			{
				skybox.material = dimensionSkybox;	
			}
		}
	}

	#endregion
}

public delegate void DimensionEventHandler(object sender, DimensionEventArgs e);

public class DimensionEventArgs : System.EventArgs
{
	#region Properties

	public Dimension fromDimension { get; private set; }
	public Dimension toDimension { get; private set; }

	#endregion

	#region Constructors

	public DimensionEventArgs(Dimension fromDimension, Dimension toDimension)
	{
		this.fromDimension = fromDimension;
		this.toDimension = toDimension;
	}

	#endregion
}