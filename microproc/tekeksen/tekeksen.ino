#include "HX711.h"
#include <Wire.h>
#include <Adafruit_ADS1015.h>

// HX711 circuit wiring
const int LOADCELL_DOUT_PIN = 2;
const int LOADCELL_SCK_PIN = 3;
long reading=0;

HX711 scale;

float calibration_factor = -30;

double lvdt1=2553; // LVDT dijital okuma 1
double lvdt2=10175; // LVDT dijital okuma 1
double okuma1=0; //kumpas gerçek okuma 1
double okuma2=7.88; // kumpas gerçek okuma 2*/
double sifirlama=-22500;

Adafruit_ADS1115 ads(0x48);

void setup() {
  Serial.begin(57600);
  ads.begin();
  scale.begin(LOADCELL_DOUT_PIN, LOADCELL_SCK_PIN);
  delay(400);
  sifirlama = scale.read_average(40);
}

void loop() {


  int16_t adc0;
  adc0 = ads.readADC_SingleEnded(0);

  double okumalvdt=((adc0-lvdt1)/(lvdt2-lvdt1))*(okuma2-okuma1)+okuma1;
  
  if (scale.is_ready()) {
    reading = (scale.read()-sifirlama);
  } else {
    
  }
  Serial.print(okumalvdt);
  Serial.print(" mm \t");
  
  Serial.print(reading=reading/210);
  Serial.println(" kg ");


  delay(200);
  
}
