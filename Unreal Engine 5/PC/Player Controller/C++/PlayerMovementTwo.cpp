// Fill out your copyright notice in the Description page of Project Settings.


#include "PlayerMovementTwo.h"
#include "CollisionQueryParams.h"

// Sets default values
APlayerMovementTwo::APlayerMovementTwo()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void APlayerMovementTwo::BeginPlay()
{
	Super::BeginPlay();
}

// Called every frame
void APlayerMovementTwo::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void APlayerMovementTwo::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
	PlayerInputComponent->BindAxis(TEXT("MoveForward"), this, &APlayerMovementTwo::MoveForward);
	PlayerInputComponent->BindAxis(TEXT("MoveRight"), this, &APlayerMovementTwo::MoveRight);
	PlayerInputComponent->BindAxis(TEXT("Turn"), this, &APlayerMovementTwo::AddControllerYawInput);
	PlayerInputComponent->BindAxis(TEXT("LookUp"), this, &APlayerMovementTwo::AddControllerPitchInput);
	PlayerInputComponent->BindAction(TEXT("Jump"), IE_Pressed, this, &APlayerMovementTwo::Jump);
	PlayerInputComponent->BindAction(TEXT("Fire"), IE_Pressed, this, &APlayerMovementTwo::Shoot);
}

void APlayerMovementTwo::MoveForward(float AxisVal)
{
	AddMovementInput(GetActorForwardVector() * AxisVal);
}

void APlayerMovementTwo::MoveRight(float AxisVal)
{
	AddMovementInput(GetActorRightVector() * AxisVal);
}

void APlayerMovementTwo::Shoot()
{
	GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Red, TEXT("CLICK"));
	FHitResult* Hit = new FHitResult();

	FVector StartCast = APlayerMovementTwo::GetActorLocation();
	FVector Forward = APlayerMovementTwo::GetActorForwardVector();
	FVector EndCast = (Forward * 5000.0f) + StartCast;
	FCollisionQueryParams* col = new FCollisionQueryParams();

	if (GetWorld()->LineTraceSingleByChannel(*Hit, StartCast, EndCast,ECC_Visibility, *col))
	{
		GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Red, TEXT("SHOT"));
		DrawDebugLine(GetWorld(), StartCast, EndCast, FColor(255, 0, 0), true);

		if (Hit->GetActor() != NULL)
		{
			if (Hit->GetComponent()->ComponentHasTag("NoneDestructable"))
			{
				GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Green, TEXT("CAN'T DESTROY NONE DESTRUCTABLE OBJECT"));
			}
			else
			{
				FString t = Hit->GetActor()->GetDebugName(Hit->GetActor());
				GEngine->AddOnScreenDebugMessage(-1, 3, FColor::Green, FString::Printf(TEXT("HIT %s"), *t));
				Hit->GetActor()->Destroy();
			}
		}
	}
}
