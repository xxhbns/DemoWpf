using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrism.ViewModels
{
    public partial class MemoUCViewModel : ObservableValidator
    {
        public MemoUCViewModel()
        {
            CreateMemoInfoDTOList();
        }


        [ObservableProperty]
        private List<MemoInfoDTO> _memoInfoDTOList;

        private void CreateMemoInfoDTOList()
        {
            MemoInfoDTOList = [];
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoDTOList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
        }

        [ObservableProperty]
        private bool _isShowAddMemo;

        [RelayCommand]
        private void ShowAddMemo()
        {
            IsShowAddMemo = true;
        }
    }
}
