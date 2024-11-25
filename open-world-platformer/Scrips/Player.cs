using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Konstanter
	public const float Speed = 300.0f;
	public const float JumpVelocity = -450.0f;

	// Variabler til hop
	private int jumpCount = 0; // Holder styr på antallet af hop
	private const int maxJumpCount = 2; // Maksimalt antal hop (dvs. dobbelt hop)
	private bool isOnGround = true; // Tjek om karakteren er på jorden

	// Animation
	AnimatedSprite2D aniSprite;

	// Gravity
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		GD.Print("Player is ready");
		aniSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print(aniSprite);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Tjek om karakteren er på jorden
		isOnGround = IsOnFloor();

		// Tilføj tyngdekraften, når karakteren ikke er på jorden
		if (!isOnGround)
		{
			velocity.Y += gravity * (float)delta;
		}
		else
		{
			// Nulstil hop-tælleren, når karakteren lander
			jumpCount = 0;
		}

		// Håndter hop
		if (Input.IsActionJustPressed("ui_accept") && (isOnGround || jumpCount < maxJumpCount))
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			//aniSprite.Stop();        	
		//aniSprite.Play("Jump"); 
		}

		// Bevægelse og animation
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction.X > 0)
		{
			aniSprite.FlipH = false;
		}
		else if (direction.X < 0)
		{
			aniSprite.FlipH = true;
		}

		// Forskellige animationer (idle, walk, jump)
		if (isOnGround)
		{
			if (direction.X == 0)
			{
				aniSprite.Play("Idle");
			}
			else
			{
				aniSprite.Play("Idle");
			}
		}
		else
		{
			aniSprite.Play("Jump");
		}

		// Bevægelseslogik
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
