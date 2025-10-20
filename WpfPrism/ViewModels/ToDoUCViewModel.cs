using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;
using Prism.Services.Dialogs;

namespace WpfPrism.ViewModels
{
    public partial class ToDoUCViewModel : ObservableValidator
    {
        public ToDoUCViewModel()
        {
            CreateWaitInfoDTOList();
        }

        [ObservableProperty]
        private List<WaitInfoDTO> _waitInfoDTOList;

        private void CreateWaitInfoDTOList()
        {
            WaitInfoDTOList = [];
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoDTOList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
        }

        [ObservableProperty]
        private bool _isShowAddWait;

        [RelayCommand]
        private void ShowAddWait() 
        {
            IsShowAddWait = true;
        }
    }
}
