package com.condimentalgames.hide.androidmodule;

import android.util.Log;

public class HideMainActivity {
    private static final HideMainActivity ourInstance = new HideMainActivity();
    private static final String LOGTAG = "Hide";

    private final long startTime;

    public static HideMainActivity getInstance() {
        return ourInstance;
    }

    private HideMainActivity() {
        Log.i(LOGTAG, "Created HideMainActivity");
        startTime = System.currentTimeMillis();
    }

    public double getElapsedTime() {
        return (System.currentTimeMillis() - startTime) / 1000.0f;
    }
}
