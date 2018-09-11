// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeriodData.cs" company="urb31075">
// All Right Reserved  
// </copyright>
// <summary>
//   The col.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MilestoneReportDAL
{
    /// <summary>
    /// The col.
    /// </summary>
    /// <typeparam name="T">
    /// Обобщенный тип для колонки
    /// </typeparam>
    public class PeriodData<T>
    {
        #region Поля

        /// <summary>
        /// Gets or sets the name 1.
        /// </summary>
        public T V1 { get; set; }

        /// <summary>
        /// Gets or sets the name 2.
        /// </summary>
        public T V2 { get; set; }

        /// <summary>
        /// Gets or sets the name 3.
        /// </summary>
        public T V3 { get; set; }

        /// <summary>
        /// Gets or sets the name 4.
        /// </summary>
        public T V4 { get; set; }

        /// <summary>
        /// Gets or sets the name 5.
        /// </summary>
        public T V5 { get; set; }

        /// <summary>
        /// Gets or sets the name 6.
        /// </summary>
        public T V6 { get; set; }

        /// <summary>
        /// Gets or sets the name 7.
        /// </summary>
        public T V7 { get; set; }

        /// <summary>
        /// Gets or sets the name 8.
        /// </summary>
        public T V8 { get; set; }

        /// <summary>
        /// Gets or sets the name 9.
        /// </summary>
        public T V9 { get; set; }

        /// <summary>
        /// Gets or sets the name 10.
        /// </summary>
        public T V10 { get; set; }

        /// <summary>
        /// Gets or sets the name 11.
        /// </summary>
        public T V11 { get; set; }

        /// <summary>
        /// Gets or sets the name 12.
        /// </summary>
        public T V12 { get; set; }

        /// <summary>
        /// Gets or sets the name 1.
        /// </summary>
        public T V13 { get; set; }

        /// <summary>
        /// Gets or sets the name 2.
        /// </summary>
        public T V14 { get; set; }

        /// <summary>
        /// Gets or sets the name 3.
        /// </summary>
        public T V15 { get; set; }

        /// <summary>
        /// Gets or sets the name 4.
        /// </summary>
        public T V16 { get; set; }

        /// <summary>
        /// Gets or sets the name 5.
        /// </summary>
        public T V17 { get; set; }

        /// <summary>
        /// Gets or sets the name 6.
        /// </summary>
        public T V18 { get; set; }

        /// <summary>
        /// Gets or sets the name 7.
        /// </summary>
        public T V19 { get; set; }

        /// <summary>
        /// Gets or sets the name 8.
        /// </summary>
        public T V20 { get; set; }

        /// <summary>
        /// Gets or sets the name 9.
        /// </summary>
        public T V21 { get; set; }

        /// <summary>
        /// Gets or sets the name 10.
        /// </summary>
        public T V22 { get; set; }

        /// <summary>
        /// Gets or sets the name 11.
        /// </summary>
        public T V23 { get; set; }

        /// <summary>
        /// Gets or sets the name 12.
        /// </summary>
        public T V24 { get; set; }

        #endregion

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetValue(int index, T value)
        {
            switch (index)
            {
                case 1:
                    this.V1 = value;
                    break;
                case 2:
                    this.V2 = value;
                    break;
                case 3:
                    this.V3 = value;
                    break;
                case 4:
                    this.V4 = value;
                    break;
                case 5:
                    this.V5 = value;
                    break;
                case 6:
                    this.V6 = value;
                    break;
                case 7:
                    this.V7 = value;
                    break;
                case 8:
                    this.V8 = value;
                    break;
                case 9:
                    this.V9 = value;
                    break;
                case 10:
                    this.V10 = value;
                    break;
                case 11:
                    this.V11 = value;
                    break;
                case 12:
                    this.V12 = value;
                    break;
                case 13:
                    this.V13 = value;
                    break;
                case 14:
                    this.V14 = value;
                    break;
                case 15:
                    this.V15 = value;
                    break;
                case 16:
                    this.V16 = value;
                    break;
                case 17:
                    this.V17 = value;
                    break;
                case 18:
                    this.V18 = value;
                    break;
                case 19:
                    this.V19 = value;
                    break;
                case 20:
                    this.V20 = value;
                    break;
                case 21:
                    this.V21 = value;
                    break;
                case 22:
                    this.V22 = value;
                    break;
                case 23:
                    this.V23 = value;
                    break;
                case 24:
                    this.V24 = value;
                    break;
            }
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return $"{this.V1}     " + $"{this.V2}     " + $"{this.V3}     " + $"{this.V4}     " 
                 + $"{this.V5}     " + $"{this.V6}     " + $"{this.V7}     " + $"{this.V8}     " 
                 + $"{this.V9}     " + $"{this.V10}     " + $"{this.V11}     " + $"{this.V12}     " 
                 + $"{this.V13}     " + $"{this.V14}     " + $"{this.V15}     " + $"{this.V16}     " 
                 + $"{this.V17}     " + $"{this.V18}     " + $"{this.V19}     " + $"{this.V20}     "
                 + $"{this.V21}     " + $"{this.V22}     " + $"{this.V23}     " + $"{this.V24}";
        }
    }
}
