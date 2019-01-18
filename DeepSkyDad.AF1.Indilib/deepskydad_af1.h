/*
    Deep Sky Dad AF1
    Based on Moonline driver.
    Copyright (C) 2013-2019 Jasem Mutlaq (mutlaqja@ikarustech.com)

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

*/

#pragma once

#include "indifocuser.h"

#include <chrono>

class DeepSkyDadAF1 : public INDI::Focuser
{
    public:
        DeepSkyDadAF1();
        virtual ~DeepSkyDadAF1() override = default;

        typedef enum { FULL, HALF, QUARTER, EIGHT } FocusStepMode;

         typedef enum { ALWAYS_ON_NO, ALWAYS_ON_YES } AlwaysOn;

        const char * getDefaultName() override;
        virtual bool initProperties() override;
        virtual bool updateProperties() override;
        virtual bool ISNewSwitch(const char * dev, const char * name, ISState * states, char * names[], int n) override;
        virtual bool ISNewNumber(const char * dev, const char * name, double values[], char * names[], int n) override;

        static void timedMoveHelper(void * context);
    protected:
       /*
        * @brief Handshake Try to communicate with Focuser and see if there is a valid response.
        * @return True if communication is successful, false otherwise.
        */
       virtual bool Handshake() override;

       /**
        * @brief MoveFocuser Move focuser in a specific direction and speed for period of time.
        * @param dir Direction of motion. Inward or Outward
        * @param speed Speed of motion
        * @param duration Timeout in milliseconds.
        * @return IPS_BUSY if motion is in progress. IPS_OK if motion is small and already complete. IPS_ALERT for trouble.
        */
       virtual IPState MoveFocuser(FocusDirection dir, int speed, uint16_t duration) override;

       /**
        * @brief MoveAbsFocuser Move to an absolute target position
        * @param targetTicks target position
        * @return IPS_BUSY if motion is in progress. IPS_OK if motion is small and already complete. IPS_ALERT for trouble.
        */
       virtual IPState MoveAbsFocuser(uint32_t targetTicks) override;

       /**
        * @brief MoveRelFocuser Move focuser for a relative amount of ticks in a specific direction
        * @param dir Directoin of motion
        * @param ticks steps to move
        * @return IPS_BUSY if motion is in progress. IPS_OK if motion is small and already complete. IPS_ALERT for trouble.
        */
       virtual IPState MoveRelFocuser(FocusDirection dir, uint32_t ticks) override;

       /**
        * @brief SyncFocuser Set the supplied position as the current focuser position
        * @param ticks target position
        * @return IPS_OK if focuser position is now set to ticks. IPS_ALERT for problems.
        */
       virtual bool SyncFocuser(uint32_t ticks) override;

       virtual bool SetFocuserSpeed(int speed) override;
       virtual bool AbortFocuser() override;
       virtual void TimerHit() override;
       virtual bool saveConfigItems(FILE * fp) override;

    private:
        bool Ack();
        /**
         * @brief sendCommand Send a string command to DeepSkyDad focuser.
         * @param cmd Command to be sent in format "[CMD]"
         * @param res If not nullptr, the function will read until it detects the response in format "(RES)"
         *        if nullptr, no read back is done and the function returns true.
         * @return True if successful, false otherwise.
         */
        bool sendCommand(const char * cmd, char * res = nullptr);
        bool sendCommandSet(const char * cmd);

        // Get initial focuser parameter when we first connect
        void GetFocusParams();
        bool readStepMode();
        bool readPosition();
        bool readSpeed();
        bool readSettleBuffer();
        bool readAlwaysOn();
        bool isMoving();

        void timedMoveCallback();

        bool setStepMode(FocusStepMode mode);
        bool setSettleBuffer(uint32_t position);
        bool setAlwaysOn(AlwaysOn ao);

        bool MoveFocuser(uint32_t position);


        double targetPos { 0 }, lastPos { 0 }, lastSettleBuffer { 0 };

        int settleBuffer { 0 };

        // Step modes
        ISwitch StepModeS[4];
        ISwitchVectorProperty StepModeSP;

        // Always on
        ISwitch AlwaysOnS[2];
        ISwitchVectorProperty AlwaysOnSP;

        // Settle buffer
        INumber SettleBufferN[1];
        INumberVectorProperty SettleBufferNP;

        // Response Buffer
        
        static const uint8_t DSD_RES { 32 };
        static const char DSD_DEL { ')' };
        static const uint8_t DSD_TIMEOUT { 3 };
};
