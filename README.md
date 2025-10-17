# .NET MAUI Location Heatmap App

A .NET MAUI application that tracks a user's location and displays it as a real-time heatmap on a map. This project uses the built-in MAUI Maps component, SQLite for local storage, and a custom `GraphicsView` to render the heatmap overlay.

![App Screenshot](https://drive.google.com/file/d/1sJc8f4qvRy30LWEMP12y4UoL787mgnwC/view?usp=sharing)

## Features

-   **Live Location Tracking:** Captures location updates in the background.
-   **Persistent Storage:** Saves location points to a local SQLite database.
-   **Heatmap Visualization:** Renders saved locations as a heatmap layer on the map.
-   **Map Controls:** Start, stop, and clear tracking data.

## Prerequisites

-   Visual Studio 2022 (or later)
-   .NET 8 SDK
-   **.NET Multi-platform App UI development** workload installed via the Visual Studio Installer.

## Setup & Configuration

**IMPORTANT:** This project uses Google Maps, which requires an API key.

1.  **Get a Google Maps API Key:**
    * Follow the guide here: [Get an API Key](https://developers.google.com/maps/documentation/android-sdk/get-api-key).
    * Make sure the **Maps SDK for Android** is enabled for your key in the Google Cloud Console.

2.  **Add the API Key to the Android Manifest:**
    * Open the file: `LocationHeatmap/Platforms/Android/AndroidManifest.xml`.
    * Inside the `<application>` tag, add the following line, replacing `YOUR_API_KEY_HERE` with your actual key:
        ```xml
        <meta-data android:name="com.google.android.geo.API_KEY" android:value="YOUR_API_KEY_HERE" />
        ```

## How to Run

1.  Clone or download this repository.
2.  Open the `LocationHeatmap.sln` file in Visual Studio.
3.  Complete the API Key **Setup** step above.
4.  Set the startup project to **Android Emulator**.
5.  Press the "Run" button (green play icon) to build and deploy the app to your selected emulator.
6.  Once the app launches, grant location permissions when prompted.
7.  Use the emulator's extended controls to simulate location changes and see the heatmap appear.
