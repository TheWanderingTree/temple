using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Landmark : IComparable<Landmark> 
{

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
	public Direction DirectionChunk;							// Subdivision of compass rose covering 45 degrees
	public int Bearing;											// Compass bearing the landmark is located along (N, NE, E, SE, S, SW, W, NW)

	public uint? AudioEventName;								// Sound effect associated with the landmark in periscope view

	// CLASS CONSTRUCTORS

	public Landmark( string title, string description, int tier )
	{
		Title = title;
		Description = description;
		Tier = tier;
	}

	public Landmark( string title, string description, int tier, uint audioEventName )
	{
		Title = title;
		Description = description;
		Tier = tier;
		AudioEventName = audioEventName;
	}


	// CLASS METHODS

	// compares landmarks by distance
	public int CompareTo (Landmark other) 
	{						
	
		if (other == null) {
			return 1;
		}

		return Distance - other.Distance;
	}
	
	// adds name, description, bearing, tier and audio preview of the landmark to the journal
	public void addToJournal() 
	{								
	
	}

	// transition from ocean walk to landmark
	public void enterLandmark()	
	{								
		Visited = true;
	}

	// transition from landmark back to ocean walk
	public void exitLandmark() 
	{								

	}

	// prints information about landmark
	public void Print() 
	{										
		Debug.Log (Title.ToUpper () + ": " + Description + "\n" + "TIER " + Tier + " / " + Distance + "m / " + Bearing);
		if (Visited) { Debug.Log ("Visited\n"); }
		if (Completed) { Debug.Log ("Completed\n"); }
		if (Hidden) { Debug.Log ("Hidden\n"); }
		Debug.Log ("=============================================\n");
	}

	void Start () 
	{

	}

}
	
public class LandmarkManager : Singleton<LandmarkManager> 
{	
	public List<Landmark> tier1Landmarks = new List<Landmark>();
	public List<Landmark> tier2Landmarks = new List<Landmark>();
	public List<Landmark> tier3Landmarks = new List<Landmark>();

	public T selectRandomItem<T>( List<T> list) 
	{
		int chosenIndex = UnityEngine.Random.Range (0, list.Count-1);
		T chosenItem = list[chosenIndex]; 

		return chosenItem;
	}

	public T pluckRandomItem<T>( List<T> list) 
	{
		int chosenIndex = UnityEngine.Random.Range (0, list.Count-1);
		T chosenItem = list[chosenIndex]; 
		list.RemoveAt (chosenIndex);
		return chosenItem;
	}

	public List<Landmark> getLandmarksFromTier( int tier) 
	{

		List<Landmark> chosenList = null;

		switch (tier) 
		{
			case 1:
				chosenList = LandmarkManager.Instance.tier1Landmarks;
				break;
			
			case 2:
				chosenList = LandmarkManager.Instance.tier2Landmarks;
				break;
			
			case 3:
				chosenList = LandmarkManager.Instance.tier3Landmarks;
				break;
		}

		return chosenList;
	}

	// randomizes bearings and distances for all landmarks in a certain list, then sorts based on distance
	void randomizeLandmarks( List<Landmark> list ) 
	{			

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

		foreach (Landmark landmark in list) 
		{

			//assign random direction chunk to landmark
			landmark.DirectionChunk = selectRandomItem(availableBearings);

			switch (landmark.DirectionChunk) 
			{

			case Landmark.Direction.N:
				int rightOfNorth;
				rightOfNorth = UnityEngine.Random.Range (0,1);
				if (rightOfNorth == 0) 
				{
					landmark.Bearing = UnityEngine.Random.Range(338,359);
				} 
				else 
				{
					landmark.Bearing = UnityEngine.Random.Range (0,23);
				}
				break;
			case Landmark.Direction.NE:
				landmark.Bearing = UnityEngine.Random.Range (23, 68);
				break;
			case Landmark.Direction.E:
				landmark.Bearing = UnityEngine.Random.Range (68, 113);
				break;
			case Landmark.Direction.SE:
				landmark.Bearing = UnityEngine.Random.Range (113, 158);
				break;
			case Landmark.Direction.S:
				landmark.Bearing = UnityEngine.Random.Range (158, 203);
				break;
			case Landmark.Direction.SW:
				landmark.Bearing = UnityEngine.Random.Range (203, 248);
				break;
			case Landmark.Direction.W:
				landmark.Bearing = UnityEngine.Random.Range (248, 293);
				break;
			case Landmark.Direction.NW:
				landmark.Bearing = UnityEngine.Random.Range (293, 338);
				break;

			}

			//pluck a chunk from the available chunks for that bearing and remove a bearing from the list of available bearings if two chunks have already been taken
			if (!landmark.Endgate) 
			{

				//randomize chunk limit
				int chunkLimit = UnityEngine.Random.Range(4,5);

				switch (landmark.DirectionChunk) 
				{

					case Landmark.Direction.N:
						landmark.DistanceChunk = pluckRandomItem(availableNorth);
					if (availableNorth.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.N); }
						break;

					case Landmark.Direction.NE:
						landmark.DistanceChunk = pluckRandomItem(availableNortheast);
					if (availableNortheast.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.NE); }
						break;

					case Landmark.Direction.E:
						landmark.DistanceChunk = pluckRandomItem(availableEast);
					if (availableEast.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.E); }
						break;

					case Landmark.Direction.SE:
						landmark.DistanceChunk = pluckRandomItem(availableSoutheast);
					if (availableSoutheast.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.SE); }
						break;

					case Landmark.Direction.S:
						landmark.DistanceChunk = pluckRandomItem(availableSouth);
					if (availableSouth.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.S); }
						break;
						
					case Landmark.Direction.SW:
						landmark.DistanceChunk = pluckRandomItem(availableSouthwest);
					if (availableSouthwest.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.SW); }
						break;
						
					case Landmark.Direction.W:
						landmark.DistanceChunk = pluckRandomItem(availableWest);
					if (availableWest.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.W); }
						break;
						
					case Landmark.Direction.NW:
						landmark.DistanceChunk = pluckRandomItem(availableNorthwest);
					if (availableNorthwest.Count <= chunkLimit) { availableBearings.Remove(Landmark.Direction.NW); }
						break;
				} 

				// assign a random distance within that chunk to the landmark
				switch (landmark.DistanceChunk) 
				{

					case Landmark.Chunk.A:
						landmark.Distance = UnityEngine.Random.Range(50, 75);
						break;
			
					case Landmark.Chunk.B:
						landmark.Distance = UnityEngine.Random.Range(75, 100);
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

		}

		list.Sort ();
	}

	void Start () 
	{

		Landmark wreck = new Landmark(
			"Wreck" ,
			"Get wrecked at a wrecked ship." ,
			1,
			AK.EVENTS.PREVIEWWRECK
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
			1,
			AK.EVENTS.PREVIEWPLATEAU
			);

		Landmark seamount = new Landmark(
			"Seamount" ,
			"An underwater mountain." ,
			1,
			AK.EVENTS.PREVIEWSEAMOUNT
			);

		Landmark whitesmokers = new Landmark(
			"Hydrothermal Vents" ,
			"Small, white chimney structures spewing white, superheated fluids." ,
			1 
			);

		Landmark canyon = new Landmark(
			"Canyon" ,
			"Cliffs with dozens of windows carved out of the walls." ,
			1	
			);
		
		
		Landmark statue = new Landmark(
			"Statue" ,
			"A statue in the shape of the youth's head." ,
			1 
			);
		
		Landmark well = new Landmark(
			"Well" ,
			"A dark hole in the seafloor, about two meters wide." ,
			1,
			AK.EVENTS.PREVIEWWELL
			);
		
		Landmark torpedo = new Landmark(
			"Torpedo" ,
			"An undetonated torpedo." ,
			1 
			);
		
		Landmark nothing = new Landmark(
			"Nothing" ,
			"There is nothing here. Nothing at all." ,
			1,
			AK.EVENTS.PREVIEWNOTHING
			);
		
		Landmark oldsuit = new Landmark(
			"Old Diving Suit" ,
			"A canvas diving suit from the late 19th century." ,
			1 
			);

		Landmark trench = new Landmark(
			"Trench" ,
			"Like the canyon, but worse." ,
			2 
			);
		
		Landmark brinepool = new Landmark(
			"Brine Pool" ,
			"A lake of high-density brine on the ocean floor." ,
			2 
			);
		
		Landmark blacksmokers = new Landmark(
			"Hydrothermal Vents" ,
			"Huge, black chimney structures formed from minerals, which spew superheated fluids that look like billowing clouds of black smoke. The liquid is supercritical; it does not have distinct liquid or gas phases. It is hot enough to melt metal." ,
			2 
			);

		Landmark dolphins = new Landmark(
			"Dolphins" ,
			"They are everywhere." ,
			2 
			);
		
		Landmark seaspiders = new Landmark(
			"Sea spiders" ,
			"Jesus christ fucking sea spiders." ,
			2 
			);
		
		Landmark giantsquid = new Landmark(
			"Giant Squid" ,
			"Because of course a giant squid." ,
			2 
			);

		Landmark cave = new Landmark(
			"Cave" ,
			"Be a slave to the CAVE!" ,
			2,
			AK.EVENTS.PREVIEWCAVE
			);

		wreck.Endgate = true; wreck.Distance = 300;
		cave.Endgate = true; cave.Distance = 300;

		
		tier1Landmarks.Add(wreck);
		tier1Landmarks.Add(lifeboat);
		tier1Landmarks.Add(minefield);
		tier1Landmarks.Add(plateau);
		tier1Landmarks.Add(seamount);
		tier1Landmarks.Add(whitesmokers);
		tier1Landmarks.Add(canyon);
		tier1Landmarks.Add(statue);
		tier1Landmarks.Add(well);
		tier1Landmarks.Add(torpedo);
		tier1Landmarks.Add(nothing);
		tier1Landmarks.Add(oldsuit);

		tier2Landmarks.Add(trench);
		tier2Landmarks.Add(brinepool);
		tier2Landmarks.Add(blacksmokers);
		tier2Landmarks.Add(dolphins);
		tier2Landmarks.Add(seaspiders);
		tier2Landmarks.Add(giantsquid);
		tier2Landmarks.Add(cave);


		randomizeLandmarks (tier1Landmarks);
		randomizeLandmarks (tier2Landmarks);


		/* DEBUG INFO FOR LANDMARKS 
		foreach (Landmark landmark in tier1Landmarks) {
		
			landmark.Print();

		}

		foreach (Landmark landmark in tier2Landmarks) {
			
			landmark.Print();
			
		} */

	}

}
