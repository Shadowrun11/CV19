﻿using CV19.Infrastructure.Commands.Base;
using CV19.Models;
using CV19.Models.Decanat;
using CV19.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /*--------------------------------------------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; }

        public object[] CompositeCollection { get; }

        /// <summary> Выбранный непонятный элемент </summary>
        private object _SelectedCompositeValue;

        /// <summary> Выбранный непонятный элемент </summary>
        public object SelectedCompositeValue
        {
            get => _SelectedCompositeValue;
            set => Set(ref _SelectedCompositeValue, value);
        }

        #region SelectedGroup : Group - Номер выбранной вкладки 

        /// <summary> Выбранная группа </summary>
        private Group _SelectedGroup;

        /// <summary> Выбранная группа </summary>
        public Group SelectedGroup
        {
            get => _SelectedGroup;
            set => Set(ref _SelectedGroup, value);
        }

        #endregion

        #region SelectedPageIndex : int - Номер выбранной вкладки 

        /// <summary> Номер выбранной вкладки </summary>
        private int _SelectedPageIndex = 0;

        /// <summary> Номер выбранной вкладки </summary>
        public int SelectedPageIndex
        {
            get => _SelectedPageIndex;
            set => Set(ref _SelectedPageIndex, value);
        }

        #endregion

        #region TestDatePoints: IEnumerable<DatePoint> - Тествый набор данных для визуализации графиков

        /// <summary> DESCRIPTION </summary>
        private IEnumerable<DataPoint> _TestDatePoints;

        /// <summary> DESCRIPTION </summary>
        public IEnumerable<DataPoint> TestDatePoints
        {
            get => _TestDatePoints;
            set => Set(ref _TestDatePoints, value);
        }

        #endregion
        #region Заголовок окна
        private string _Title = "Анализ статистики CV19";

        ///<summary> Заголовок окна </summary>
        public string Title
        {
            get => _Title;
            //set
            //{
            //if (Equals(_Title, value)) return;
            //_Title = value;
            //OnPropertyChanged();

            // Set(ref _Title, value);
            //}
            set => Set(ref _Title, value);

        }
        #endregion

        #region Status : string - Статус программы

        /// <summary>Статус программы</summary>
        private string _Status = "Готово!";

        ///<summary>Статус программы</summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        /*--------------------------------------------------------------------------------------------------------------*/

        #region Команды

        /*--------------------------------------------------------------------------------------------------------------*/

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region ChangeTabIndexCommand
        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommandExecute(object p) => _SelectedPageIndex >= 0;

        private void OnChangeTabIndexCommandExecute(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }
        #endregion

        #region CreateGroupCommand
        public ICommand CreateGroupCommand { get; }

        private bool CanCreateGroupCommandExecute(object p) => true;

        private void OnCreateGroupCommandExecute(object p)
        {
            int group_max_index = Groups.Count + 1;
            var new_group = new Group
            {
                Name = $"Группа {group_max_index}",
                Students = new ObservableCollection<Student>()
            };

            Groups.Add(new_group);
        }
        #endregion

        #region DeleteGroupCommandand
        public ICommand DeleteGroupCommand { get; }
        private bool CanDeleteGroupCommandandExecute(object p) => p is Group group && Groups.Contains(group);

        private void OnDeleteGroupCommandandExecute(object p)
        {
            if (!(p is Group group)) return;
            var group_index = Groups.IndexOf(group);
            Groups.Remove(group);
            if (group_index < Groups.Count)
                SelectedGroup = Groups[group_index];
        }
        #endregion

        #endregion


        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecute, CanChangeTabIndexCommandExecute);
            CreateGroupCommand = new LambdaCommand(OnCreateGroupCommandExecute, CanCreateGroupCommandExecute);
            DeleteGroupCommand = new LambdaCommand(OnDeleteGroupCommandandExecute, CanDeleteGroupCommandandExecute);
            
            #endregion

            var date_points = new List<DataPoint>((int)(360 / 0.1)); 
            for(var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(2 * Math.PI * x * to_rad);

                date_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDatePoints = date_points;


            var student_index = 1;

            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {student_index}",
                Surname = $"Surname {student_index}",
                Patronymic = $"Patronymic {student_index++}",
                Birthday = DateTime.Now,
                Rating = 0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);

            var data_list = new List<object>();

            data_list.Add("Hello World");
            data_list.Add(42);
            var group = Groups[1];
            data_list.Add(group);
            data_list.Add(group.Students[0]);

            CompositeCollection = data_list.ToArray();
        }
    }
}
