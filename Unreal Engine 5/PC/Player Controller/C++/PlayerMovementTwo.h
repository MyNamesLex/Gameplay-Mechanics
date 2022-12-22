// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "PlayerMovementTwo.generated.h"

UCLASS()
class PLAYER_API APlayerMovementTwo : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	APlayerMovementTwo();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

protected:
	void MoveForward(float AxisVal);
	void MoveRight(float AxisVal);
	void Shoot();
};
