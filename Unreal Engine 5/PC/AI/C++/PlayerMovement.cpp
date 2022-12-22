// Fill out your copyright notice in the Description page of Project Settings.


#include "PlayerMovement.h"

// Sets default values
APlayerMovement::APlayerMovement()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
}

// Called when the game starts or when spawned
void APlayerMovement::BeginPlay()
{
	Super::BeginPlay();
}

// Called every frame
void APlayerMovement::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void APlayerMovement::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
	PlayerInputComponent->BindAxis("Horizontal", this, &APlayerMovement::HorizontalMovement);
	PlayerInputComponent->BindAxis("Vertical", this, &APlayerMovement::VerticalMovement);
	PlayerInputComponent->BindAxis("Turn", this, &APlayerMovement::AddControllerYawInput);
	PlayerInputComponent->BindAxis("LookUp", this, &APlayerMovement::AddControllerPitchInput);
	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &APlayerMovement::Jump);
	PlayerInputComponent->BindAction("Shoot", IE_Pressed, this, &APlayerMovement::Shoot);
}

void APlayerMovement::HorizontalMovement(float val)
{
	AddMovementInput(GetActorRightVector(), val);
}

void APlayerMovement::VerticalMovement(float val)
{
	AddMovementInput(GetActorForwardVector(), val);
}

void APlayerMovement::Shoot()
{
	FHitResult* Hit = new FHitResult();
	FVector StartCast = cam;
	FVector Forward = camForward;
	FVector EndCast = (Forward * 5000.0f) + StartCast;
	FCollisionQueryParams* col = new FCollisionQueryParams();

	if (GetWorld()->LineTraceSingleByChannel(*Hit, StartCast, EndCast, ECC_Visibility, *col))
	{
		DrawDebugLine(GetWorld(), StartCast, EndCast, FColor(255, 0, 0));
		if (Hit->GetActor() != NULL)
		{
			if (Hit->GetComponent()->ComponentHasTag("EnemyAI"))
			{
				GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Blue, TEXT("HIT ENEMY AI"));
			}
			else
			{
				GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Blue, TEXT("HIT ACTOR"));
			}
		}
	}
}