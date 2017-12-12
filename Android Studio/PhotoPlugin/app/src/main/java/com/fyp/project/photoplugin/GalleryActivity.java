package com.fyp.project.photoplugin;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;


public class GalleryActivity extends Activity {

    static final int GALLERY_REQUEST = 4596;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        openGallery();
    }

    public void openGallery() {
        Intent galleryIntent = new Intent(Intent.ACTION_GET_CONTENT);
        galleryIntent.setType("image/*");
        if (galleryIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(galleryIntent, GALLERY_REQUEST);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        try {
            if (requestCode == GALLERY_REQUEST && resultCode == Activity.RESULT_OK) {
                Uri pickedPhotoLocation = data.getData();
                Bitmap bitmap = null;

                InputStream inputStream = null;
                try {
                    inputStream = getContentResolver().openInputStream(pickedPhotoLocation);
                    bitmap = BitmapFactory.decodeStream(inputStream);
                    inputStream.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }

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
                PhotoPlugin.sendMessageToPhotoPluginObject("Gallery_Done", json_array);

            } else {
                super.onActivityResult(requestCode, resultCode, data);
            }
        } finally {
            PhotoPlugin.pluginObject.backToUnity(this);
        }
    }

}
