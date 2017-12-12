package com.fyp.project.photoplugin;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.provider.MediaStore;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.ByteArrayOutputStream;


public class CameraActivity extends Activity {

    static final int CAMERA_REQUEST = 2375;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        openCamera();
    }

    public void openCamera() {
        Intent cameraIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        if (cameraIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(cameraIntent, CAMERA_REQUEST);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        try {
            if (requestCode == CAMERA_REQUEST && resultCode == Activity.RESULT_OK && data != null) {
                Bundle extras = data.getExtras();
                Bitmap bitmap = (Bitmap) extras.get("data");

                ByteArrayOutputStream stream = new ByteArrayOutputStream();
                if(bitmap != null)
                    bitmap.compress(Bitmap.CompressFormat.JPEG, 100, stream);
                byte[] byteArray = stream.toByteArray();

                JSONObject json_obj = new JSONObject();
                try {
                    json_obj.put("width", bitmap.getWidth());
                    json_obj.put("height", bitmap.getHeight());
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                JSONArray json_array = new JSONArray();
                json_array.put(json_obj);

                PhotoPlugin.pluginObject.setPluginData(byteArray);
                PhotoPlugin.sendMessageToPhotoPluginObject("Camera_Done", json_array);

            } else {
                super.onActivityResult(requestCode, resultCode, data);
            }
        } finally {
            PhotoPlugin.pluginObject.backToUnity(this);
        }
    }

}
