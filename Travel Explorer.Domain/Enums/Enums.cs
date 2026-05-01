using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Domain.Enums
{
    public enum BookingStatus
    {
        Pending = 1,    // حجز مسجل و لم يدفع
        Confirmed = 2,  // تم الدفع وتأكيد الحجز
        Cancelled = 3,  // تم الإلغاء من العميل أو السيستم
        Completed = 4,  // الرحلة انتهت فعلياً
        Refunded = 5    // تم الإلغاء واسترداد المبلغ
    }

    public enum PaymentStatus
    {
        Unpaid = 1,
        Partial = 2,    // ( في حالة العربون ( اختيارى
        Paid = 3,
        Failed = 4,
        Refunded = 5
    }

    public enum FlightClass
    {
        Economy = 1,
        Business = 2,
        FirstClass = 3
    }

    public enum UserRole
    {
        Admin = 1,
        Traveler = 2,
        Author = 3 // للـ Blog
    }
}
