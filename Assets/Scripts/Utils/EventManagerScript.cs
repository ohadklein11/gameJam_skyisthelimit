using UnityEngine.Events;
using System.Collections.Generic;

public class EventManagerScript : Singleton<EventManagerScript>
{
	public const string GiantFightStart = "GiantFightStart";
	public const string GiantDoorsOpen = "GiantDoorsOpen";
	public const string LeavingGiantTemple = "LeavingGiantTemple";
	public const string GiantFightEnd = "GiantFightEnd";
	public const string GiantAttitude = "GiantAttitude";
	public const string PlayerGotHit = "PlayerGotHit";
	public const string HealthRecovery = "HealthRecovery";


	protected EventManagerScript()
    {
        Init();
    } // guarantee this will be always a singleton only - can't use the constructor!

    public class FloatEvent : UnityEvent<object> {} //empty class; just needs to exist


    private Dictionary <string, FloatEvent> eventDictionary;

    private void Init ()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, FloatEvent>();
		}
	}
	
	public void StartListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.AddListener (listener);
		} 
		else
		{
			thisEvent = new FloatEvent ();
			thisEvent.AddListener (listener);
			eventDictionary.Add (eventName, thisEvent);
		}
	}
	
	public void StopListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.RemoveListener (listener);
		}
	}
	
	public void TriggerEvent (string eventName, object obj)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.Invoke (obj);
		}
	}
}
