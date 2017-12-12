package com.fyp.project.photoplugin;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import com.unity3d.player.UnityPlayer;
import org.json.JSONArray;


public class PhotoPlugin {
    public static PhotoPlugin pluginObject = new PhotoPlugin();
    public Context unityActivity;
    public static final String GAMEOBJECT = "GameObject";  // Unity GameObject Name
    private byte[] pluginData = null;

    /* Get the plugin data (= image data) */
    public byte[] getPluginData(){
        return pluginData;
    }

    /* Set the plugin data with image data */
    public void setPluginData(byte[] data){
        pluginData = data;
    }

    /* Clear all data */
    public void cleanUp(){
        pluginData = null;
        unityActivity = null;
    }

    /* Start phone's camera */
    public void startCamera(Context context){
        this.unityActivity = context;
        Intent myIntent = new Intent(context, CameraActivity.class);
        context.startActivity(myIntent);
    }

    /* Start phone's gallery */
    public void startGallery(Context context){
        this.unityActivity = context;
        Intent myIntent = new Intent(context, GalleryActivity.class);
        context.startActivity(myIntent);
    }

    /* Go back to Unity (from Android Activity) */
    public void backToUnity(Activity androidActivity){
        Intent myIntent = new Intent(androidActivity, unityActivity.getClass());
        unityActivity.startActivity(myIntent);
    }

    /* Send message to Unity's GameObject (named as "GameObject") */
    public static void sendMessageToPhotoPluginObject(String method, JSONArray parameters){
        UnityPlayer.UnitySendMessage(GAMEOBJECT, method, parameters.toString());
    }

}
