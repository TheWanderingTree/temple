using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Landmark : IComparable<Landmark> {

	// CLASS FIELDS
	
	public bool Visited = false;								// True if the landmark has been visited by the player
	public bool Completed = false;								// True if the landmark has been fully explored by the player
	public bool Hidden = false;									// True if the landmark is only "shown" in periscope with the searchlight turned off
	public bool Endgate = false;								// True if the landmark is an endgate for the tier it's located in
	
	public int Tier;											// Tier the landmark is located in (1-3)
	public int Distance;										// Distance the landmark is at within a tier
	
	public string Title;										// Title of the landmark
	public string Description;									// Description of the landmark

	public enum Chunk { A, B, C, D, E, F } 						// Enumeration of distance chunks
	public Chunk DistanceChunk;									// Subdivision of total tier distance the landmark is located within

	public enum Direction { N, NE, E, SE, S, SW, W, NW }		// Enumeration for compass directions
	public Direction Bearing;									// Compass bearing the landmark is located along (N, NE, E, SE, S, SW, W, NW)

	public AudioClip AudioPreview;								// Sound effect associated with the landmark in periscope view

	// CLASS CONSTRUCTORS

	public Landmark( string title, string description, int tier )
	{
		Title = title;
		Description = description;
		Tier = tier;
	}

	public Landmark( string title, string description, int tier, AudioClip audioPreview )
	{
		Title = title;
		Description = description;
		Tier = tier;
		AudioPreview = audioPreview;
	}


	// CLASS METHODS

	public int CompareTo (Landmark other) {						// compares landmarks by distance
	
		if (other == null) {
			return 1;
		}

		return Distance - other.Distance;
	}
	
	public void addToJournal() {								// adds name, description, bearing, tier and audio preview of the landmark to the journal
	

	}

	public void enterLandmark()	{								// transition from ocean walk to landmark
	
		Visited = true;
	}

	public void exitLandmark() {								// transition from landmark back to ocean walk
	

	}

	public void Print() {										// prints information about landmark
	
		Debug.Log (Title.ToUpper () + ": " + Description + "\n" + "TIER " + Tier + " / " + Distance + "m / " + Bearing);
		if (Visited) { Debug.Log ("Visited\n"); }
		if (Completed) { Debug.Log ("Completed\n"); }
		if (Hidden) { Debug.Log ("Hidden\n"); }
		Debug.Log ("=============================================\n");
	}

	void Start () {
	

	}

}
	
public class LandmarkManager : Singleton<LandmarkManager> {	

	public List<Landmark> tier1Landmarks = new List<Landmark>();

	public T selectRandomItem<T>( List<T> list) {
		int chosenIndex = UnityEngine.Random.Range (0, list.Count);
		T chosenItem = list[chosenIndex]; 

		return chosenItem;
	}

	public T pluckRandomItem<T>( List<T> list) {
		int chosenIndex = UnityEngine.Random.Range (0, list.Count);
		T chosenItem = list[chosenIndex]; 
		list.RemoveAt (chosenIndex);
		return chosenItem;
	}

	void randomizeLandmarks( List<Landmark> list ) {			// randomizes bearings and distances for all landmarks in a certain list, then sorts based on distance

		//create list of available bearings
		List<Landmark.Direction> availableBearings = Enum.GetValues (typeof(Landmark.Direction)).Cast<Landmark.Direction>().ToList ();

		//create lists of available chunks for each direction

		Landmark.Chunk[] chunks = Enum.GetValues(typeof(Landmark.Chunk)).Cast<Landmark.Chunk>().ToArray();
		List<Landmark.Chunk> availableNorth = new List<Landmark.Chunk> (); availableNorth.AddRange(chunks);
		List<Landmark.Chunk> availableNortheast = new List<Landmark.Chunk> (); availableNortheast.AddRange(chunks);
		List<Landmark.Chunk> availableEast = new List<Landmark.Chunk> (); availableEast.AddRange(chunks);
		List<Landmark.Chunk> availableSoutheast = new List<Landmark.Chunk> (); availableSoutheast.AddRange(chunks);
		List<Landmark.Chunk> availableSouth = new List<Landmark.Chunk> (); availableSouth.AddRange(chunks);
		List<Landmark.Chunk> availableSouthwest = new List<Landmark.Chunk> (); availableSouthwest.AddRange(chunks);
		List<Landmark.Chunk> availableWest = new List<Landmark.Chunk> (); availableWest.AddRange(chunks);
		List<Landmark.Chunk> availableNorthwest = new List<Landmark.Chunk> (); availableNorthwest.AddRange(chunks);

		foreach (Landmark landmark in list) {

			//assign random bearing to landmark
			landmark.Bearing = selectRandomItem(availableBearings);

			//pluck a chunk from the available chunks for that bearing and assign a random distance within that chunk to the landmark
			if (!landmark.Endgate) {
			switch (landmark.Bearing) {

				case Landmark.Direction.N:
					landmark.DistanceChunk = pluckRandomItem(availableNorth);
					break;

				case Landmark.Direction.NE:
					landmark.DistanceChunk = pluckRandomItem(availableNortheast);
					break;

				case Landmark.Direction.E:
					landmark.DistanceChunk = pluckRandomItem(availableEast);
					break;

				case Landmark.Direction.SE:
					landmark.DistanceChunk = pluckRandomItem(availableSoutheast);
					break;

				case Landmark.Direction.S:
					landmark.DistanceChunk = pluckRandomItem(availableSouth);
					break;
					
				case Landmark.Direction.SW:
					landmark.DistanceChunk = pluckRandomItem(availableSouthwest);
					break;
					
				case Landmark.Direction.W:
					landmark.DistanceChunk = pluckRandomItem(availableWest);
					break;
					
				case Landmark.Direction.NW:
					landmark.DistanceChunk = pluckRandomItem(availableNorthwest);
					break;
			} 

			switch (landmark.DistanceChunk) {

				case Landmark.Chunk.A:
					landmark.Distance = UnityEngine.Random.Range(15, 50);
					break;
		
				case Landmark.Chunk.B:
					landmark.Distance = UnityEngine.Random.Range(50, 100);
					break;
				
				case Landmark.Chunk.C:
					landmark.Distance = UnityEngine.Random.Range(100, 150);
					break;
				
				case Landmark.Chunk.D:
					landmark.Distance = UnityEngine.Random.Range(150, 200);
					break;

				case Landmark.Chunk.E:
					landmark.Distance = UnityEngine.Random.Range(200, 250);
					break;

				case Landmark.Chunk.F:
					landmark.Distance = UnityEngine.Random.Range(250, 285);
					break;

			}	
			}

			//if landmarks in a bearing = 6, remove bearing from available bearings
		}


//		foreach (Landmark landmark in list) {
//			landmark.randomizeBearing ();
//			landmark.randomizeDistance ();
//		}

		list.Sort ();
	}

	void Start () {

		Landmark wreck = new Landmark(
			"Wreck" ,
			"Get wrecked at a wrecked ship." ,
			1	
			);
		
		
		Landmark lifeboat = new Landmark(
			"Lifeboat" ,
			"Dead bodies and splintered wood." ,
			1 
			);
		
		Landmark minefield = new Landmark(
			"Minefield" ,
			"Ticking underwater mines want to blow you apart." ,
			1 
			);

		Landmark plateau = new Landmark(
			"Oceanic Plateau" ,
			"A high plateau on the seafloor covered in whale carcasses." ,
			1 
			);

		Landmark seamount = new Landmark(
			"Seamount" ,
			"An underwater mountain." ,
			1 
			);

		Landmark whitesmokers = new Landmark(
			"Hydrothermal Vents" ,
			"Small, white chimney structures spewing white, superheated fluids." ,
			1 
			);
		
		wreck.Endgate = true; wreck.Distance = 300;

		
		tier1Landmarks.Add(wreck);
		tier1Landmarks.Add(lifeboat);
		tier1Landmarks.Add(minefield);
		tier1Landmarks.Add(plateau);
		tier1Landmarks.Add(seamount);
		tier1Landmarks.Add(whitesmokers);

		randomizeLandmarks (tier1Landmarks);



		foreach (Landmark landmark in tier1Landmarks) {
		
			landmark.Print();

		}

	}

}
