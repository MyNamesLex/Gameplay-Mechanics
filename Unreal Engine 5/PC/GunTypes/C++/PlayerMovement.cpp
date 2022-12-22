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
	PlayerInputComponent->BindAxis(TEXT("MoveForward"), this, &APlayerMovement::MoveForward);
	PlayerInputComponent->BindAxis(TEXT("MoveRight"), this, &APlayerMovement::MoveRight);
	PlayerInputComponent->BindAxis(TEXT("Turn"), this, &APlayerMovement::AddControllerYawInput);
	PlayerInputComponent->BindAxis(TEXT("LookUp"), this, &APlayerMovement::AddControllerPitchInput);
	PlayerInputComponent->BindAction(TEXT("Jump"), IE_Pressed, this, &APlayerMovement::Jump);
}

void APlayerMovement::MoveForward(float val)
{
	AddMovementInput(GetActorForwardVector() * val);
}

void APlayerMovement::MoveRight(float val)
{
	AddMovementInput(GetActorRightVector() * val);
}

