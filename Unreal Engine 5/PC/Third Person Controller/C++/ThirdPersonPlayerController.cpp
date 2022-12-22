// Fill out your copyright notice in the Description page of Project Settings.


#include "ThirdPersonPlayerController.h"

// Sets default values
AThirdPersonPlayerController::AThirdPersonPlayerController()
{
	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AThirdPersonPlayerController::BeginPlay()
{
	Super::BeginPlay();

}

// Called every frame
void AThirdPersonPlayerController::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void AThirdPersonPlayerController::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &AThirdPersonPlayerController::Jump);
	PlayerInputComponent->BindAxis("TurnRight", this, &AThirdPersonPlayerController::AddControllerYawInput);
	PlayerInputComponent->BindAxis("LookUp", this, &AThirdPersonPlayerController::AddControllerPitchInput);
}