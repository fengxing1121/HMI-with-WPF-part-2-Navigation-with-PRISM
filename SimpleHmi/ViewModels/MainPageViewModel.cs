﻿using Prism.Commands;
using Prism.Mvvm;
using SimpleHmi.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleHmi.ViewModels
{
    class MainPageViewModel : BindableBase
    {
        public string IpAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }
        private string _ipAddress;

        public bool HighLimit
        {
            get { return _highLimit; }
            set { SetProperty(ref _highLimit, value); }
        }
        private bool _highLimit;

        public bool LowLimit
        {
            get { return _lowLimit; }
            set { SetProperty(ref _lowLimit, value); }
        }
        private bool _lowLimit;

        public bool PumpState
        {
            get { return _pumpState; }
            set { SetProperty(ref _pumpState, value); }
        }
        private bool _pumpState;

        public int TankLevel
        {
            get { return _tankLevel; }
            set { SetProperty(ref _tankLevel, value); }
        }
        private int _tankLevel;

        

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisconnectCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        IPlcService _plcService;

        public MainPageViewModel(IPlcService s7PlcService)
        {
            _plcService = s7PlcService;
            ConnectCommand = new DelegateCommand(Connect);
            DisconnectCommand = new DelegateCommand(Disconnect);
            StartCommand = new DelegateCommand(async () => { await Start(); });
            StopCommand = new DelegateCommand(async () => { await Stop(); });

            IpAddress = "127.0.0.1";

            OnPlcServiceValuesRefreshed(null, null);
            _plcService.ValuesRefreshed += OnPlcServiceValuesRefreshed;
        }

        private void OnPlcServiceValuesRefreshed(object sender, EventArgs e)
        {            
            PumpState = _plcService.PumpState;
            HighLimit = _plcService.HighLimit;
            LowLimit = _plcService.LowLimit;
            TankLevel = _plcService.TankLevel;            
        }

        private void Connect()
        {
            _plcService.Connect(IpAddress, 0, 1);
        }

        private void Disconnect()
        {
            _plcService.Disconnect();
        }

        private async Task Start()
        {
            await _plcService.WriteStart();
        }

        private async Task Stop()
        {
            await _plcService.WriteStop();
        }
    }
}
