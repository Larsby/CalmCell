using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopParticleSystem : MonoBehaviour
{



	public float multiplier = 1;

	private ParticleSystem[] m_Systems;
	public bool toggle = false;
	private bool do_enabled = true;
	private bool started = false;
	public bool StartDisabled = false;



	void Start ()
	{
		m_Systems = GetComponentsInChildren<ParticleSystem> ();
		if (m_Systems == null) {
			m_Systems = GetComponents<ParticleSystem> ();
		
		}
		if (StartDisabled) {
			SetEnabled (false);
		} else {
			SetEnabled (true);
		}
	}

	public void SetEnabledFast (bool do_enable, float val)
	{
		if (do_enable && started == false) {
			if (gameObject.transform.childCount > 0) {
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
			}
			started = true;
			m_Systems = GetComponentsInChildren<ParticleSystem> ();
		}
		foreach (ParticleSystem system in m_Systems) {
			var emission = system.emission;
			if (do_enable == false) {
				var main = system.main;
				main.simulationSpeed = val;
				system.Stop ();
			} else {
				var main = system.main;
				main.simulationSpeed = val;

				system.Play ();
			}
			//emission.enabled = do_enable;
		}
	}

	public void SetEnabled (bool do_enable)
	{
		if (do_enable && started == false) {
			if (gameObject.transform.childCount > 0) {
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
			}
			started = true;
			m_Systems = GetComponentsInChildren<ParticleSystem> ();
		}
		foreach (ParticleSystem system in m_Systems) {
			var emission = system.emission;
			if (do_enable == false) {

				system.Stop ();
			} else {


				system.Play ();
			}
			//emission.enabled = do_enable;
		}
	}

	public void Extinguish ()
	{
		SetEnabled (false);
	}

	public void Ignite (float delay)
	{
		SetEnabled (true);
		Invoke ("Extinguish", delay);
	}


	// Update is called once per frame
	void Update ()
	{ 
		if (toggle) {
			toggle = false;
			do_enabled = !do_enabled;
			SetEnabled (do_enabled);
		}
	}
}