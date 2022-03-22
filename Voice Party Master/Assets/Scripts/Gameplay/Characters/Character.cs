using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterClass {
	Warrior, Rogue, Archer, Mage, Priest
}

public enum PrimaryDamageType {
	Physical, Magical
}

public class Character
{	
	// Actions
	public Dictionary<string, Delegate> Actions = new Dictionary<string, Delegate>();	
    
	// Variables
	public bool isSelected = false;
	protected string characterName;
	protected CharacterStats base_stats;
	protected PrimaryDamageType damageType;

	protected Animator animator;

	// Getters
	public string GetName() {return characterName; }	 
	public CharacterStats GetStats() { return base_stats; }
	public PrimaryDamageType GetDamageType() { return damageType; }

	// Setters
	public void SetAnimator(Animator reference) {
		animator = reference;
	}

	// Static Functions
	public static Character Load(CharacterClass characterClass) 
	{
		switch (characterClass) {
			default:
			case CharacterClass.Warrior:
				return new Warrior();
			case CharacterClass.Rogue:
				return new Rogue();
			case CharacterClass.Archer:
				return new Archer();
			case CharacterClass.Mage:
				return new Mage();
			case CharacterClass.Priest:
				return new Priest(); 
		}
	}
	

}
