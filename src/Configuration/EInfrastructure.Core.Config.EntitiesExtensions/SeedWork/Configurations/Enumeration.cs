﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniqueCommon = EInfrastructure.Core.Config.EntitiesExtensions.Common.UniqueCommon;

namespace EInfrastructure.Core.Config.EntitiesExtensions.SeedWork.Configurations
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public abstract class Enumeration<T1, T2> : IComparable
        where T1 : IComparable
        where T2 : IComparable
    {
        /// <summary>
        ///
        /// </summary>
        public T1 Id { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public T2 Name { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        protected Enumeration(T1 id, T2 name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name?.ToString() ?? "";

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration<T1, T2>
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<T1, T2>;

            if (otherValue == null)
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => UniqueCommon.GetHashCode(Id.ToString());

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromValue<T>(T1 value) where T : Enumeration<T1, T2>
        {
            var matchingItem = Parse<T, T1>(value, "value", item => item.Id.Equals(value));
            return matchingItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="displayName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromDisplayName<T>(string displayName) where T : Enumeration<T1, T2>
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name.Equals(displayName));
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate)
            where T : Enumeration<T1, T2>
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            return matchingItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(object other) => Id.CompareTo(((Enumeration<T1, T2>) other).Id);
    }
}