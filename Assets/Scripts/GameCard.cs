using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCard : MonoBehaviour
{

	public CardDef Definition;

	bool doOnce = false;
	public int targetStack = -1;
	public int sourceStack = -1;
	public bool isTouch = true;
	private float ResetAnimationDuration = 0.45f;
	// State for card being dealt and flying out of deck.
	bool m_winning = false;
	bool m_flip = false;
	bool m_flying;
	bool m_selecting = false;
	bool m_resetAnimation = false;
	float m_flyTime;
	float m_flyDuration;
	Vector3 m_flySource;
	private Vector3 m_flyTarget;
	MegaModifyObject modObj;
	MegaPointCache bendObj;

	private float rotcompx = 0;
	private Animator animator;
	private bool bToFoundation = false;
	private bool m_backflip = false;

	private bool lateEnd = false, lateEndSecond = false;

	public void SetToFoundation() {
		bToFoundation = true;
	}

	public void SetMeTarget () {
		m_flyTarget = this.gameObject.transform.position;
	}


	public void SetFlyTarget (Vector3 source, Vector3 target, float duration, bool flip, bool backflip=false)
	{
		m_flying = true;
		m_flyTime = Time.time;
		m_flyDuration = duration;
		m_flySource = source;
		m_flyTarget = target;
		m_flip = flip;
		m_backflip = backflip;

		if (flip == true)
			PlayFlipAnim (backflip);
		else if (duration > 0)
			PlayDropAnim ();
	}

	public Vector3 GetFlySource ()
	{
		return m_flySource;
	}

	public void PlayFlipAnim(bool backflip = false) {
		if (modObj.enabled == false) {
			modObj.enabled = true;
			animator.enabled = true;
			if (!backflip)
				animator.Play("flip");
			else
				animator.Play("backflip");
		}
	}


	public void PlayLiftAnim() {
		if (modObj.enabled == false) {
			modObj.enabled = true;
			animator.enabled = true;
			animator.Play("lift");
		}
	}

	public void PlayDropAnim() {
		if (modObj != null) {
			modObj.enabled = true;
			animator.enabled = true;
			animator.Play("put_down");

//			if (!bToFoundation && targetStack < 9)
				iTween.RotateTo (this.gameObject, new Vector3(this.transform.rotation.x, this.transform.rotation.y, -2+Random.Range(0,5)), 0.6f);
			bToFoundation = false;
		}
	}

	public void stopAnim ()
	{
		bendObj.time = 0.45f;
		animator.Play(m_backflip? "idle_card_down_reset" : "idle_card_up");
		transform.GetChild (0).localPosition = Vector3.zero;

		lateEnd = true; // one frame later, animator is disabled in Update method. This is needed to let the "idle_card_up/down" animation run to reset card bend/rotation
	}


	public void bendMe()
	{
		bendObj.time = 0.45f + Random.Range(0, 0.5f);
		modObj.enabled = true;
	}

	public void SetSelectTarget (Vector3 source, Vector3 target, float duration)
	{
		m_flyDuration = duration;
		m_flyTime = Time.time;
		m_flySource = source;
		m_flyTarget = target;
		m_selecting = true;
	}

	public void SetResetAnimation ()
	{
		m_resetAnimation = true;
	}


	public void SetNoFly ()
	{
		m_flying = false;
	}

	public void SetFly ()
	{
		m_flying = true;
	}

	public void setWinning ()
	{
		m_winning = true;
	}

	public void setNotWinning ()
	{
		m_winning = false;
	}

	public bool isFlying ()
	{
		return m_flying;
	}

	public bool isAnimating ()
	{
		if (m_flying || m_selecting) {
			return true;
		}
		return false;
	}

	public bool isTweening() {
		return iTween.Count (gameObject) > 0;
	}

	void Start ()
	{
		bendObj = this.gameObject.GetComponentInChildren<MegaPointCache> ();
		modObj = this.gameObject.GetComponentInChildren<MegaModifyObject> ();
		modObj.enabled = false;
		animator = GetComponentInChildren<Animator> ();
		animator.enabled = false;

		animator.enabled = true;
		stopAnim ();
	}

	public void FlipCard (Vector3 axis) {
	}

	// Update is called once per frame
	void Update ()
	{
		bool bPosition = true;

		if (lateEndSecond) { // for some inexplicable reason we need to run an extra frame with bend on, or card can get stuck
			modObj.enabled = false;
			lateEndSecond = false;
		}

		if (lateEnd) {
			animator.enabled = false;
			lateEnd = false;
			lateEndSecond = true; // see above
		}


		//FLYING AND FLIPPING ANIMATION --------------------------------------------------
		if (m_flying && !m_winning) {
			float t = Time.time - m_flyTime;
			Vector3 pos = transform.position;	//Cards current world-position
			//Move card
			if (t < m_flyDuration) {
				float tt = t / m_flyDuration;

				if (m_flip) {
					float mt = Mathf.Clamp01 (tt - 0.5f) / 0.5f; // start move late
					if (m_backflip)
						mt = Mathf.Clamp01 (tt - 0.3f) / 0.7f; // start move late
					pos = Vector3.Lerp (m_flySource, m_flyTarget, mt);
				} else { //ANIMATION FOR NON FLIPPING CARDS
					pos = Vector3.Lerp (m_flySource, m_flyTarget, tt);
				}
				doOnce = true;

				  
			} else { //when time's up

				if (!m_resetAnimation)
					stopAnim ();

				m_flying = false;

				//Round
				m_flyTarget.x = (float)System.Math.Round (m_flyTarget.x, 2);
				m_flyTarget.y = (float)System.Math.Round (m_flyTarget.y, 2);
				m_flyTarget.z = (float)System.Math.Round (m_flyTarget.z, 2);

				pos = m_flyTarget;

				if ((targetStack < 7 && targetStack > -1) || (targetStack == -1 && sourceStack == -1))
					Definition.FaceUp = true;

				//Organize parentstack when animationen is finished
				if (doOnce) {
//					Debug.Log ("EXTRA ORGANIZE AFTER ANIM READY! Stack=" + this.Definition.Stack.ToString ());

					UpdateSourceStack ();

					StartCoroutine (shortWait (0.1f)); //Wait a bit so target and source aren't updated simultaneously

//					Debug.Log ("TargetStack=" + targetStack.ToString ());
					this.transform.position = pos;
					bPosition = false;
					if (!m_backflip) {
						UpdateTargetStack ();
						bPosition = true;
					}
					m_backflip = false;

					doOnce = false;

					//Only needed for debugging. No...this fixes so selectstack gets reset Z pos too
					GameObject.Find ("KlondikeGame").GetComponent<KlondikeGame> ().ResetSelectStack ();
				}
			}

			if (m_resetAnimation) {
				PlayDropAnim ();

//				iTween.EaseType et = iTween.Defaults.easeType;
//				iTween.Defaults.easeType = iTween.EaseType.linear;
				iTween.MoveTo (gameObject, pos, ResetAnimationDuration);
//				iTween.Defaults.easeType = et;

//				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", ResetAnimationDuration, "easetype", iTween.EaseType.linear));

				Invoke ("stopAnim", ResetAnimationDuration);
				m_resetAnimation = false;
			} else {
				if (bPosition)
					this.transform.position = pos;
			}
		}	


		// SELECT ANIMATION --------------------------------------
		if (m_selecting) {	//Animation when selecting (clicking on) card
			float t = Time.time - m_flyTime;
			Vector3 pos = transform.position;	
			 
			if (t < m_flyDuration) {
				pos = Vector3.Lerp (m_flySource, m_flyTarget, t);
	
			} else { //when time's up
				pos = m_flyTarget;
				m_selecting = false;
			}
			this.transform.position = pos;
		}

		// WINNING ANIMATION --------------------------------------
		if (m_winning) {	//This animation is only used when game is won

			if (!animator.enabled) {
				animator.enabled = true;
				modObj.enabled = true;
				animator.Play ("victory");
				MeshRenderer mr = GetComponentInChildren <MeshRenderer>();
				mr.transform.localScale = new Vector3 (-mr.transform.localScale.x, -mr.transform.localScale.y, mr.transform.localScale.z);
				Invoke ("Hide", 2);
			}
		}	
	}
		
	private void Hide() {
		modObj.enabled = false;
		animator.enabled = false;
		this.gameObject.SetActive (false);
	}

	private void UpdateTargetStack ()
	{
		if (targetStack != -1) {
			if (this.targetStack < 8) {
				this.GetComponentInParent<ColumnStack> ().OrganizeStack (false, isTouch, true);
			}
			if (this.targetStack == 8) {
//				Debug.Log ("EXTRA - REMAINING");
				this.GetComponentInParent<RemainingStack> ().OrganizeStack ();
			}
			if (this.targetStack == 8) {
//				Debug.Log ("EXTRA - WASTE");
				this.GetComponentInParent<WasteStack> ().OrganizeStack ();
			}
			if (this.targetStack > 8) {
//				Debug.Log ("EXTRA - FOUNDATION");
				this.GetComponentInParent<FoundationStack> ().OrganizeStack ();
			}
		}
	}

	private void UpdateSourceStack ()
	{


		if (sourceStack != -1) {
			if (this.sourceStack < 8) {
				GameObject.Find ("ColumnStack" + sourceStack.ToString ()).GetComponent<ColumnStack> ().OrganizeStack (true, isTouch, false);
			}
			if (this.sourceStack == 8) {
				GameObject.Find ("RemainingStack").GetComponent<RemainingStack> ().OrganizeStack ();
			}
			if (this.sourceStack == 8) {
				GameObject.Find ("WasteStack").GetComponent<WasteStack> ().OrganizeStack ();
			}
			if (this.sourceStack > 8) {
				GameObject.Find ("FoundationStack" + (sourceStack - 8).ToString ()).GetComponent<FoundationStack> ().OrganizeStack ();
			}
		}
	}

	public void FlipCard ()
	{
		Quaternion rot = transform.rotation;
		rot = Quaternion.Euler (0 + rotcompx, this.transform.rotation.y + 180, rot.z);		//-10 because it's flipped
		this.transform.rotation = rot;
	}

	public void SelectCard () //Don't need this anymore
	{
		 
	}

	public void UnSelectCard () //Don't need this anymore
	{
 
	}

 
	/****************/


	IEnumerator shortWait (float in_delay)
	{
		yield return new WaitForSeconds (in_delay);
	}
}

