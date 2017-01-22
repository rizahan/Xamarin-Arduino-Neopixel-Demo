#include <SoftwareSerial.h>
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif
#define PIN            6
#define NUMPIXELS      24 
char c = ' ';
int a=222;
int red=0;
int green=0;
int blue=0;
SoftwareSerial BTSerial(2, 3); 
Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);


void setup() 
{
   #if defined (__AVR_ATtiny85__)
     if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
    #endif
      pixels.begin(); 
      Serial.begin(9600);
      Serial.println("Sketch HC-0x_FC-114_01_9600");
      Serial.println("Arduino with HC-0x FC-114 is ready");
      Serial.println("Make sure Both NL & CR are set");
      Serial.println("");
      
      // FC-114 default baud rate is 9600
      BTSerial.begin(9600);  
      Serial.println("BTserial started at 9600");
      Serial.println("");
      
}

void loop()
{

    
    if (BTSerial.available())
    {
        a = BTSerial.read();
          Serial.println(a);
         if(a==250)
        {
          c='R';
          a=0;
        }
         if(a==251)
        {
          c='G';
          a=0;
        }
         if(a==252)
        {
          c='B';
          a=0;
        }
        if(a==253)
        {
           colorWipe(pixels.Color(255, 0, 0), 50); // Red
  colorWipe(pixels.Color(0, 255, 0), 50); // Green
  colorWipe(pixels.Color(0, 0, 255), 50); // Blue
//colorWipe(pixels.Color(0, 0, 0, 255), 50); // White RGBW
  // Send a theater pixel chase in...
  theaterChase(pixels.Color(127, 127, 127), 50); // White
  theaterChase(pixels.Color(127, 0, 0), 50); // Red
  theaterChase(pixels.Color(0, 0, 127), 50); // Blue

  rainbow(20);
  rainbowCycle(20);
  theaterChaseRainbow(50);
        }
        if(a==254)
        {for(int i=0;i<NUMPIXELS;i++){
           pixels.setPixelColor(i, pixels.Color(0,0,0));
        }
          a=0;
        }
      
for(int i=0;i<NUMPIXELS;i++){

  if(c=='R')
  {
    red=a;
  }
  if(c=='G')
  {
    green=a;
  }
   if(c=='B')
  {
    blue=a;
  }

 pixels.setPixelColor(i, pixels.Color(red,green,blue));
  
  }
    


    pixels.show(); // This sends the updated pixel color to the hardware.



  }
        
       
    }
// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i<pixels.numPixels(); i++) {
    pixels.setPixelColor(i, c);
    pixels.show();
    delay(wait);
  }
}

void rainbow(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256; j++) {
    for(i=0; i<pixels.numPixels(); i++) {
      pixels.setPixelColor(i, Wheel((i+j) & 255));
    }
    pixels.show();
    delay(wait);
  }
}

    // Slightly different, this makes the rainbow equally distributed throughout
void rainbowCycle(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256*5; j++) { // 5 cycles of all colors on wheel
    for(i=0; i< pixels.numPixels(); i++) {
      pixels.setPixelColor(i, Wheel(((i * 256 / pixels.numPixels()) + j) & 255));
    }
    pixels.show();
    delay(wait);
  }
}

//Theatre-style crawling lights.
void theaterChase(uint32_t c, uint8_t wait) {
  for (int j=0; j<10; j++) {  //do 10 cycles of chasing
    for (int q=0; q < 3; q++) {
      for (uint16_t i=0; i < pixels.numPixels(); i=i+3) {
        pixels.setPixelColor(i+q, c);    //turn every third pixel on
      }
      pixels.show();

      delay(wait);

      for (uint16_t i=0; i < pixels.numPixels(); i=i+3) {
        pixels.setPixelColor(i+q, 0);        //turn every third pixel off
      }
    }
  }
}

//Theatre-style crawling lights with rainbow effect
void theaterChaseRainbow(uint8_t wait) {
  for (int j=0; j < 256; j++) {     // cycle all 256 colors in the wheel
    for (int q=0; q < 3; q++) {
      for (uint16_t i=0; i < pixels.numPixels(); i=i+3) {
        pixels.setPixelColor(i+q, Wheel( (i+j) % 255));    //turn every third pixel on
      }
      pixels.show();

      delay(wait);

      for (uint16_t i=0; i < pixels.numPixels(); i=i+3) {
        pixels.setPixelColor(i+q, 0);        //turn every third pixel off
      }
    }
  }
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
  WheelPos = 255 - WheelPos;
  if(WheelPos < 85) {
    return pixels.Color(255 - WheelPos * 3, 0, WheelPos * 3);
  }
  if(WheelPos < 170) {
    WheelPos -= 85;
    return pixels.Color(0, WheelPos * 3, 255 - WheelPos * 3);
  }
  WheelPos -= 170;
  return pixels.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
}
      
  
  


