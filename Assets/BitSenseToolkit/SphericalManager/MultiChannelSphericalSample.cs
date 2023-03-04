using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A collection of MultiLayerSphericalSample
public class MultiChannelSphericalSample { //Channels: 0-center, 1-left, 2-right 

    Dictionary<string, MultiLayerSphericalSample> channels;
    public float lookAtAngle;

    public MultiChannelSphericalSample(float lookAtAngle) {
        channels = new Dictionary<string, MultiLayerSphericalSample>();
        this.lookAtAngle = lookAtAngle;
    }

    public void addChannel(string name, Texture2D texture, Vector2 coords) {
        channels[name] = (new MultiLayerSphericalSample(texture, coords, lookAtAngle));
    }

    public MultiLayerSphericalSample getChannel(string channel) {
        if (channels.ContainsKey(channel)) return channels[channel];
        return null;
    }

}







//A class for storing a single input point with data for multiple layers
public class MultiLayerSphericalSample{
    public static int[,] soblelX = { {-1, 0, 1} ,
                                     {-2, 0, 2} ,
                                     {-1, 0, 1} };
    public static int[,] soblelY = { {1, 2, 1} ,
                                     {0, 0, 0} ,
                                    {-1, -2, -1} };

    Vector2[] anglesFromGradient;// x = angle, y = slope
    float[] intensities;

    public MultiLayerSphericalSample(Texture2D texture, Vector2 coords, float lookAtAngle) {
        anglesFromGradient = new Vector2[3];
        for (int i = 0; i < 3; i++){ //Calculating vector for r, g and b
            anglesFromGradient[i] = largeSobel(texture, coords, i, lookAtAngle, 11+(texture.width/255)/2);
        }
        intensities = new float[3];
        for (int i = 0; i < 3; i++) { //Calculating vector for r, g and b
            intensities[i] = texture.GetPixel((int)coords.x, (int)coords.y)[i];
        }
    }

    public float getIntensityOfLayer(int layer) {
        return intensities[layer];
    }

    public Vector2 getDirectionOfLayer(int layer) { //Returns Vec2 angle and slope
        return anglesFromGradient[layer];
    }

    Vector2 sobel(Texture2D texture, Vector2 coords, int layer, float rotation) {
        Vector2 result = Vector2.zero;
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                result.x += texture.GetPixel((int)coords.x + x - 1, (int)coords.y + y - 1)[layer] * soblelX[x, y];
            }
        }
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                result.y += texture.GetPixel((int)coords.x + x - 1, (int)coords.y + y - 1)[layer] * soblelY[x, y];
            }
        }
        result /= 9;
        float directionRAD = Mathf.Atan2(result.y, result.x);
        float directionDEG = directionRAD * 180.0f / Mathf.PI ;
        float slope = (Mathf.Abs(result.x) + Mathf.Abs(result.y));
        return new Vector2(directionDEG - rotation, slope);
    }

    Vector2 largeSobel(Texture2D texture, Vector2 coords, int layer, float rotation, int sobelSize) {
        Vector2 result = Vector2.zero;
        for (int x = -sobelSize / 2; x <= sobelSize / 2; x++) {
            for (int y = -sobelSize / 2; y <= sobelSize / 2; y++) {
                if(x != 0 && y != 0) result.x += texture.GetPixel((int)coords.x + x, (int)coords.y + y)[layer] * (((float)x)); // / ((float)x * (float)x + (float)y * (float)y))*100.0f);
            }
        }
        for (int x = -sobelSize / 2; x <= sobelSize / 2; x++) {
            for (int y = -sobelSize / 2; y <= sobelSize / 2; y++) {
                if (x != 0 && y != 0) result.y += texture.GetPixel((int)coords.x + x, (int)coords.y + y)[layer] * (((float)y)); // / ((float)x * (float)x + (float)y * (float)y))*100.0f);
            }
        }
        float slope = (Mathf.Abs(result.x) + Mathf.Abs(result.y)) / 2 / sobelSize * sobelSize; //NW
        float localPoint = texture.GetPixel((int)coords.x, (int)coords.y)[layer];
        result /= sobelSize * sobelSize;
        //if(Mathf.Abs(result.x) <= 0.001) result.x = 0;
        //if (Mathf.Abs(result.y) <= 0.001) result.y = 0;
  
        float directionRAD = Mathf.Atan2(result.y, result.x);
        float directionDEG = directionRAD * Mathf.Rad2Deg;
        return new Vector2(Mathf.DeltaAngle(directionDEG - 90, rotation), localPoint);
    }

    Vector2 rotate(Vector2 aPoint, float aDegree) {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }
}


