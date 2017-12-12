using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;


[XmlRoot("Puzzle")]
public class LevelDataController {

	[XmlArray("Levels")]
	[XmlArrayItem("Level")]
	public List<Level> levels = new List<Level>();


	public static LevelDataController Load(string path) {

		XmlSerializer serializer = new XmlSerializer (typeof(LevelDataController));

		LevelDataController levels;

		if (!File.Exists (path)) {

			Stream reader;

			reader = new MemoryStream ((Resources.Load ("Puzzle_Initial", typeof(TextAsset)) as TextAsset).bytes);

			StreamReader textReader = new StreamReader (reader);
			levels = serializer.Deserialize (textReader) as LevelDataController;
			reader.Dispose ();

		} else {

			try {
				FileEncrypt.decrypt (path);
			} catch (Exception e) {

			} finally {
				FileStream stream = new FileStream(path, FileMode.Open);
				levels = serializer.Deserialize (stream) as LevelDataController;
				stream.Close ();
			}

		}

		return levels;

	}


	public void Save(string path) {
		
		XmlSerializer serializer = new XmlSerializer (typeof(LevelDataController));

		FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);

		serializer.Serialize(stream, this);

		stream.Close ();

		FileEncrypt.encrypt (path);

	}


}
